using FluentAssertions;
using Klc.Mutabix.Application.CurrencyAccounts.Commands;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.CurrencyAccounts;

public class CreateCurrencyAccountCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public CreateCurrencyAccountCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);

        _context.Companies.Add(new Company { Name = "Test Sirket" });
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_ShouldCreateAccountAndReturnDto()
    {
        var company = await _context.Companies.FirstAsync();
        var handler = new CreateCurrencyAccountCommandHandler(_context);
        var dto = new CreateCurrencyAccountDto(company.Id, "120-01", "Test Cari", "1234567890", "test@test.com", CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.CompanyId.Should().Be(company.Id);
        result.Code.Should().Be("120-01");
        result.Name.Should().Be("Test Cari");
        result.TaxNumber.Should().Be("1234567890");
        result.Email.Should().Be("test@test.com");
        result.CurrencyType.Should().Be(CurrencyType.TRY);
        result.IsActive.Should().BeTrue();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_WithUsdCurrency_ShouldSetCorrectType()
    {
        var company = await _context.Companies.FirstAsync();
        var handler = new CreateCurrencyAccountCommandHandler(_context);
        var dto = new CreateCurrencyAccountDto(company.Id, "320-01", "USD Cari", null, null, CurrencyType.USD);

        var result = await handler.Handle(new CreateCurrencyAccountCommand(dto), CancellationToken.None);

        result.CurrencyType.Should().Be(CurrencyType.USD);
        result.TaxNumber.Should().BeNull();
        result.Email.Should().BeNull();
    }
}
