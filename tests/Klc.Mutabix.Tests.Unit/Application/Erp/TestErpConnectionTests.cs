using FluentAssertions;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Commands;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Erp;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Application.Erp;

public class TestErpConnectionTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public TestErpConnectionTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<int> SeedConnection(ErpProviderType provider, string? apiUrl = null, string? connStr = null, string? apiKey = null)
    {
        var company = new Company { Name = "Test" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var conn = new ErpConnection
        {
            CompanyId = company.Id, Provider = provider, Name = "Test Conn",
            ApiUrl = apiUrl, ConnectionString = connStr, ApiKey = apiKey
        };
        _context.ErpConnections.Add(conn);
        await _context.SaveChangesAsync();
        return conn.Id;
    }

    [Fact]
    public async Task Handle_SapWithApiUrl_ShouldReturnTrue()
    {
        var connId = await SeedConnection(ErpProviderType.Sap, apiUrl: "https://sap.local");
        var adapters = new List<IErpAdapter> { new SapErpAdapter(Mock.Of<ILogger<SapErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SapWithoutApiUrl_ShouldReturnFalse()
    {
        var connId = await SeedConnection(ErpProviderType.Sap, apiUrl: null);
        var adapters = new List<IErpAdapter> { new SapErpAdapter(Mock.Of<ILogger<SapErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_LogoWithConnectionString_ShouldReturnTrue()
    {
        var connId = await SeedConnection(ErpProviderType.Logo, connStr: "Server=logo;Database=TIGER");
        var adapters = new List<IErpAdapter> { new LogoErpAdapter(Mock.Of<ILogger<LogoErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ParasutWithApiKey_ShouldReturnTrue()
    {
        var connId = await SeedConnection(ErpProviderType.Parasut, apiKey: "secret");
        var adapters = new List<IErpAdapter> { new ParasutErpAdapter(Mock.Of<ILogger<ParasutErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ParasutWithoutApiKey_ShouldReturnFalse()
    {
        var connId = await SeedConnection(ErpProviderType.Parasut, apiKey: null);
        var adapters = new List<IErpAdapter> { new ParasutErpAdapter(Mock.Of<ILogger<ParasutErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ExcelAlwaysTrue()
    {
        var connId = await SeedConnection(ErpProviderType.Excel);
        var adapters = new List<IErpAdapter> { new ExcelErpAdapter(Mock.Of<ILogger<ExcelErpAdapter>>()) };
        var factory = new ErpAdapterFactory(adapters);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var result = await handler.Handle(new TestErpConnectionCommand(connId), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_NonExistentConnection_ShouldThrow()
    {
        var factory = new ErpAdapterFactory([]);
        var handler = new TestErpConnectionCommandHandler(_context, factory);

        var act = () => handler.Handle(new TestErpConnectionCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*bulunamadi*");
    }
}
