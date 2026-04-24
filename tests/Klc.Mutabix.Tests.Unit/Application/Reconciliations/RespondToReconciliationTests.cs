using FluentAssertions;
using Klc.Mutabix.Application.Reconciliations.Commands;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Reconciliations;

public class RespondToReconciliationTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly RespondToReconciliationCommandHandler _handler;

    public RespondToReconciliationTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _handler = new RespondToReconciliationCommandHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private async Task<string> SeedSentAccountReconciliation()
    {
        var company = new Company { Name = "Test" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var account = new CurrencyAccount
        {
            CompanyId = company.Id, Code = "120-01", Name = "Cari",
            CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        await _context.SaveChangesAsync();

        var guid = System.Guid.NewGuid().ToString();
        _context.AccountReconciliations.Add(new AccountReconciliation
        {
            CompanyId = company.Id, CurrencyAccountId = account.Id,
            StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 3, 31),
            CurrencyType = CurrencyType.TRY, DebitAmount = 10000, CreditAmount = 8000,
            Status = ReconciliationStatus.Sent, Guid = guid, IsSent = true,
            SentDate = DateTime.UtcNow.AddDays(-1)
        });
        await _context.SaveChangesAsync();
        return guid;
    }

    private async Task<string> SeedSentBaBsReconciliation()
    {
        var company = new Company { Name = "Test BaBs" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var account = new CurrencyAccount
        {
            CompanyId = company.Id, Code = "320-01", Name = "BaBs Cari",
            CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        await _context.SaveChangesAsync();

        var guid = System.Guid.NewGuid().ToString();
        _context.BaBsReconciliations.Add(new BaBsReconciliation
        {
            CompanyId = company.Id, CurrencyAccountId = account.Id,
            Type = BaBsType.Ba, Year = 2026, Month = 3,
            Amount = 50000, Quantity = 15,
            Status = ReconciliationStatus.Sent, Guid = guid, IsSent = true
        });
        await _context.SaveChangesAsync();
        return guid;
    }

    [Fact]
    public async Task Handle_ApproveAccountReconciliation_ShouldSetStatusApproved()
    {
        var guid = await SeedSentAccountReconciliation();
        var dto = new RespondToReconciliationDto(true, "Onaylandi");
        var command = new RespondToReconciliationCommand(guid, dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        var recon = await _context.AccountReconciliations.FirstAsync();
        recon.Status.Should().Be(ReconciliationStatus.Approved);
        recon.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_RejectAccountReconciliation_ShouldSetStatusRejected()
    {
        var guid = await SeedSentAccountReconciliation();
        var dto = new RespondToReconciliationDto(false, "Tutarlar uyusmuyor");
        var command = new RespondToReconciliationCommand(guid, dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        var recon = await _context.AccountReconciliations.FirstAsync();
        recon.Status.Should().Be(ReconciliationStatus.Rejected);
        recon.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ApproveBaBsReconciliation_ShouldSetStatusApproved()
    {
        var guid = await SeedSentBaBsReconciliation();
        var dto = new RespondToReconciliationDto(true, null);
        var command = new RespondToReconciliationCommand(guid, dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        var recon = await _context.BaBsReconciliations.FirstAsync();
        recon.Status.Should().Be(ReconciliationStatus.Approved);
    }

    [Fact]
    public async Task Handle_WithInvalidGuid_ShouldReturnFalse()
    {
        var dto = new RespondToReconciliationDto(true, null);
        var command = new RespondToReconciliationCommand("invalid-guid-12345", dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }
}
