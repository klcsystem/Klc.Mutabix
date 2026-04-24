using FluentAssertions;
using Klc.Mutabix.Application.Reconciliations.Queries;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Reconciliations;

public class GetReconciliationStatsTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly GetReconciliationStatsQueryHandler _handler;

    public GetReconciliationStatsTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _handler = new GetReconciliationStatsQueryHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WithEmptyDb_ShouldReturnAllZeros()
    {
        var query = new GetReconciliationStatsQuery(1);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalAccountReconciliations.Should().Be(0);
        result.PendingAccountReconciliations.Should().Be(0);
        result.ApprovedAccountReconciliations.Should().Be(0);
        result.RejectedAccountReconciliations.Should().Be(0);
        result.TotalBaBsReconciliations.Should().Be(0);
        result.PendingBaBsReconciliations.Should().Be(0);
        result.ApprovedBaBsReconciliations.Should().Be(0);
        result.RejectedBaBsReconciliations.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithMixedStatuses_ShouldReturnCorrectCounts()
    {
        // Seed
        var company = new Company { Name = "Stats Test" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var account = new CurrencyAccount
        {
            CompanyId = company.Id, Code = "120-01", Name = "Cari",
            CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        await _context.SaveChangesAsync();

        // 3 account reconciliations: 1 Pending, 1 Approved, 1 Rejected
        _context.AccountReconciliations.AddRange(
            new AccountReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Pending, Guid = "g1"
            },
            new AccountReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Approved, Guid = "g2"
            },
            new AccountReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Rejected, Guid = "g3"
            }
        );

        // 2 BaBs: 1 Pending, 1 Approved
        _context.BaBsReconciliations.AddRange(
            new BaBsReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                Type = BaBsType.Ba, Year = 2026, Month = 1,
                Status = ReconciliationStatus.Pending, Guid = "bg1"
            },
            new BaBsReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                Type = BaBsType.Bs, Year = 2026, Month = 2,
                Status = ReconciliationStatus.Approved, Guid = "bg2"
            }
        );

        await _context.SaveChangesAsync();

        var query = new GetReconciliationStatsQuery(company.Id);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalAccountReconciliations.Should().Be(3);
        result.PendingAccountReconciliations.Should().Be(1);
        result.ApprovedAccountReconciliations.Should().Be(1);
        result.RejectedAccountReconciliations.Should().Be(1);
        result.TotalBaBsReconciliations.Should().Be(2);
        result.PendingBaBsReconciliations.Should().Be(1);
        result.ApprovedBaBsReconciliations.Should().Be(1);
        result.RejectedBaBsReconciliations.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldNotCountInactiveRecords()
    {
        var company = new Company { Name = "Inactive Test" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var account = new CurrencyAccount
        {
            CompanyId = company.Id, Code = "120-01", Name = "Cari",
            CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        await _context.SaveChangesAsync();

        _context.AccountReconciliations.AddRange(
            new AccountReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Pending, Guid = "a1", IsActive = true
            },
            new AccountReconciliation
            {
                CompanyId = company.Id, CurrencyAccountId = account.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Pending, Guid = "a2", IsActive = false
            }
        );
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(new GetReconciliationStatsQuery(company.Id), CancellationToken.None);

        result.TotalAccountReconciliations.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldFilterByCompanyId()
    {
        var company1 = new Company { Name = "Sirket 1" };
        var company2 = new Company { Name = "Sirket 2" };
        _context.Companies.AddRange(company1, company2);
        await _context.SaveChangesAsync();

        var account1 = new CurrencyAccount { CompanyId = company1.Id, Code = "C1", Name = "C1", CurrencyType = CurrencyType.TRY };
        var account2 = new CurrencyAccount { CompanyId = company2.Id, Code = "C2", Name = "C2", CurrencyType = CurrencyType.TRY };
        _context.CurrencyAccounts.AddRange(account1, account2);
        await _context.SaveChangesAsync();

        _context.AccountReconciliations.AddRange(
            new AccountReconciliation
            {
                CompanyId = company1.Id, CurrencyAccountId = account1.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Pending, Guid = "x1"
            },
            new AccountReconciliation
            {
                CompanyId = company2.Id, CurrencyAccountId = account2.Id,
                StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),
                Status = ReconciliationStatus.Approved, Guid = "x2"
            }
        );
        await _context.SaveChangesAsync();

        var result1 = await _handler.Handle(new GetReconciliationStatsQuery(company1.Id), CancellationToken.None);
        var result2 = await _handler.Handle(new GetReconciliationStatsQuery(company2.Id), CancellationToken.None);

        result1.TotalAccountReconciliations.Should().Be(1);
        result1.PendingAccountReconciliations.Should().Be(1);
        result1.ApprovedAccountReconciliations.Should().Be(0);

        result2.TotalAccountReconciliations.Should().Be(1);
        result2.ApprovedAccountReconciliations.Should().Be(1);
        result2.PendingAccountReconciliations.Should().Be(0);
    }
}
