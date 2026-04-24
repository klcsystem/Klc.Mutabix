using FluentAssertions;
using Klc.Mutabix.Application.CurrencyAccounts.Commands;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.CurrencyAccounts;

public class UpdateCurrencyAccountCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public UpdateCurrencyAccountCommandTests()
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
            CompanyId = company.Id, Code = "120-01", Name = "Eski",
            CurrencyType = CurrencyType.TRY, IsActive = true
        });
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenAccountExists_ShouldUpdateAndReturnDto()
    {
        var account = await _context.CurrencyAccounts.FirstAsync();
        var handler = new UpdateCurrencyAccountCommandHandler(_context);
        var dto = new UpdateCurrencyAccountDto("320-02", "Yeni Cari", "5555555555", "yeni@test.com", CurrencyType.EUR);
        var command = new UpdateCurrencyAccountCommand(account.Id, dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Code.Should().Be("320-02");
        result.Name.Should().Be("Yeni Cari");
        result.Email.Should().Be("yeni@test.com");
        result.CurrencyType.Should().Be(CurrencyType.EUR);

        var updated = await _context.CurrencyAccounts.FirstAsync();
        updated.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNull()
    {
        var handler = new UpdateCurrencyAccountCommandHandler(_context);
        var dto = new UpdateCurrencyAccountDto("X", "X", null, null, CurrencyType.TRY);
        var command = new UpdateCurrencyAccountCommand(999, dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();
    }
}
