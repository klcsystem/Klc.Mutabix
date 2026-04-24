using FluentAssertions;
using Klc.Mutabix.Application.CurrencyAccounts.Commands;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.CurrencyAccounts;

public class DeleteCurrencyAccountCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public DeleteCurrencyAccountCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);

        var company = new Company { Name = "Test Sirket" };
        _context.Companies.Add(company);
        _context.SaveChanges();

        _context.CurrencyAccounts.Add(new CurrencyAccount
        {
            CompanyId = company.Id, Code = "120-01", Name = "Silinecek",
            CurrencyType = CurrencyType.TRY, IsActive = true
        });
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenAccountExists_ShouldSoftDeleteAndReturnTrue()
    {
        var account = await _context.CurrencyAccounts.FirstAsync();
        var handler = new DeleteCurrencyAccountCommandHandler(_context);

        var result = await handler.Handle(new DeleteCurrencyAccountCommand(account.Id), CancellationToken.None);

        result.Should().BeTrue();
        var deleted = await _context.CurrencyAccounts.FirstAsync();
        deleted.IsActive.Should().BeFalse();
        deleted.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnFalse()
    {
        var handler = new DeleteCurrencyAccountCommandHandler(_context);

        var result = await handler.Handle(new DeleteCurrencyAccountCommand(999), CancellationToken.None);

        result.Should().BeFalse();
    }
}
