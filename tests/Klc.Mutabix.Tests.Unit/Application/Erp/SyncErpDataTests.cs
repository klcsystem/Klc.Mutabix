using FluentAssertions;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Commands;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Application.Erp;

public class SyncErpDataTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public SyncErpDataTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<int> SeedConnection()
    {
        var company = new Company { Name = "Sync Test" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var conn = new ErpConnection
        {
            CompanyId = company.Id, Provider = ErpProviderType.Generic,
            Name = "Test ERP", ApiUrl = "https://erp.local"
        };
        _context.ErpConnections.Add(conn);
        await _context.SaveChangesAsync();
        return conn.Id;
    }

    [Fact]
    public async Task Handle_SuccessfulSync_ShouldCreateSyncLogCompleted()
    {
        var connId = await SeedConnection();
        var conn = await _context.ErpConnections.FirstAsync();

        var adapterMock = new Mock<IErpAdapter>();
        adapterMock.Setup(a => a.Provider).Returns(ErpProviderType.Generic);
        adapterMock.Setup(a => a.SyncCurrencyAccountsAsync(It.IsAny<ErpConnection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ErpCurrencyAccountData>
            {
                new("120-01", "Cari A", "111", "a@t.com", CurrencyType.TRY, 10000m, 8000m),
                new("120-02", "Cari B", "222", "b@t.com", CurrencyType.USD, 5000m, 3000m)
            });

        var factoryMock = new Mock<IErpAdapterFactory>();
        factoryMock.Setup(f => f.GetAdapter(ErpProviderType.Generic)).Returns(adapterMock.Object);

        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        var result = await handler.Handle(new SyncErpDataCommand(connId), CancellationToken.None);

        result.Status.Should().Be(SyncStatus.Completed);
        result.RecordsProcessed.Should().Be(2);
        result.RecordsFailed.Should().Be(0);
        result.ErrorMessage.Should().BeNull();
        result.CompletedAt.Should().NotBeNull();

        // Check currency accounts were created
        var accounts = await _context.CurrencyAccounts.ToListAsync();
        accounts.Should().HaveCount(2);
        accounts.Should().Contain(a => a.Code == "120-01" && a.Name == "Cari A");
        accounts.Should().Contain(a => a.Code == "120-02" && a.Name == "Cari B");

        // Check connection LastSyncAt was updated
        var updatedConn = await _context.ErpConnections.FirstAsync();
        updatedConn.LastSyncAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_AdapterThrows_ShouldCreateSyncLogFailed()
    {
        var connId = await SeedConnection();

        var adapterMock = new Mock<IErpAdapter>();
        adapterMock.Setup(a => a.Provider).Returns(ErpProviderType.Generic);
        adapterMock.Setup(a => a.SyncCurrencyAccountsAsync(It.IsAny<ErpConnection>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Baglanti zaman asimina ugradi"));

        var factoryMock = new Mock<IErpAdapterFactory>();
        factoryMock.Setup(f => f.GetAdapter(ErpProviderType.Generic)).Returns(adapterMock.Object);

        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        var result = await handler.Handle(new SyncErpDataCommand(connId), CancellationToken.None);

        result.Status.Should().Be(SyncStatus.Failed);
        result.ErrorMessage.Should().Be("Baglanti zaman asimina ugradi");
        result.CompletedAt.Should().NotBeNull();
        result.RecordsProcessed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_UpsertExistingAccount_ShouldUpdateNotCreate()
    {
        var connId = await SeedConnection();
        var conn = await _context.ErpConnections.FirstAsync();

        // Pre-seed an existing account
        _context.CurrencyAccounts.Add(new CurrencyAccount
        {
            CompanyId = conn.CompanyId, Code = "120-01", Name = "Eski Isim",
            CurrencyType = CurrencyType.TRY
        });
        await _context.SaveChangesAsync();

        var adapterMock = new Mock<IErpAdapter>();
        adapterMock.Setup(a => a.Provider).Returns(ErpProviderType.Generic);
        adapterMock.Setup(a => a.SyncCurrencyAccountsAsync(It.IsAny<ErpConnection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ErpCurrencyAccountData>
            {
                new("120-01", "Yeni Isim", "111", "yeni@t.com", CurrencyType.TRY, 10000m, 8000m)
            });

        var factoryMock = new Mock<IErpAdapterFactory>();
        factoryMock.Setup(f => f.GetAdapter(ErpProviderType.Generic)).Returns(adapterMock.Object);

        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        var result = await handler.Handle(new SyncErpDataCommand(connId), CancellationToken.None);

        result.Status.Should().Be(SyncStatus.Completed);
        result.RecordsProcessed.Should().Be(1);

        // Should still be 1 account, not 2
        var accounts = await _context.CurrencyAccounts.ToListAsync();
        accounts.Should().HaveCount(1);
        accounts[0].Name.Should().Be("Yeni Isim");
        accounts[0].Email.Should().Be("yeni@t.com");
        accounts[0].UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_EmptyDataFromAdapter_ShouldCompleteWithZeroRecords()
    {
        var connId = await SeedConnection();

        var adapterMock = new Mock<IErpAdapter>();
        adapterMock.Setup(a => a.Provider).Returns(ErpProviderType.Generic);
        adapterMock.Setup(a => a.SyncCurrencyAccountsAsync(It.IsAny<ErpConnection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ErpCurrencyAccountData>());

        var factoryMock = new Mock<IErpAdapterFactory>();
        factoryMock.Setup(f => f.GetAdapter(ErpProviderType.Generic)).Returns(adapterMock.Object);

        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        var result = await handler.Handle(new SyncErpDataCommand(connId), CancellationToken.None);

        result.Status.Should().Be(SyncStatus.Completed);
        result.RecordsProcessed.Should().Be(0);
        result.RecordsFailed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_NonExistentConnection_ShouldThrow()
    {
        var factoryMock = new Mock<IErpAdapterFactory>();
        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        var act = () => handler.Handle(new SyncErpDataCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*bulunamadi*");
    }

    [Fact]
    public async Task Handle_SyncLogShouldBePersisted()
    {
        var connId = await SeedConnection();

        var adapterMock = new Mock<IErpAdapter>();
        adapterMock.Setup(a => a.Provider).Returns(ErpProviderType.Generic);
        adapterMock.Setup(a => a.SyncCurrencyAccountsAsync(It.IsAny<ErpConnection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ErpCurrencyAccountData>());

        var factoryMock = new Mock<IErpAdapterFactory>();
        factoryMock.Setup(f => f.GetAdapter(ErpProviderType.Generic)).Returns(adapterMock.Object);

        var handler = new SyncErpDataCommandHandler(_context, factoryMock.Object);

        await handler.Handle(new SyncErpDataCommand(connId), CancellationToken.None);

        var logs = await _context.ErpSyncLogs.ToListAsync();
        logs.Should().HaveCount(1);
        logs[0].ErpConnectionId.Should().Be(connId);
        logs[0].Status.Should().Be(SyncStatus.Completed);
    }
}
