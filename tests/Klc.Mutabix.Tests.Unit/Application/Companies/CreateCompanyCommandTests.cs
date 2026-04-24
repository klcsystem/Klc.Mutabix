using FluentAssertions;
using Klc.Mutabix.Application.Companies.Commands;
using Klc.Mutabix.Application.Companies.Dtos;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Companies;

public class CreateCompanyCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public CreateCompanyCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_ShouldCreateCompanyAndReturnDto()
    {
        var handler = new CreateCompanyCommandHandler(_context);
        var dto = new CreateCompanyDto("Test Sirket", "1234567890", "Kadikoy", "Istanbul");
        var command = new CreateCompanyCommand(dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test Sirket");
        result.TaxNumber.Should().Be("1234567890");
        result.TaxOffice.Should().Be("Kadikoy");
        result.Address.Should().Be("Istanbul");
        result.IsActive.Should().BeTrue();
        result.Id.Should().BeGreaterThan(0);

        var saved = await _context.Companies.FirstAsync();
        saved.Name.Should().Be("Test Sirket");
    }

    [Fact]
    public async Task Handle_WithNullableFields_ShouldCreateCompany()
    {
        var handler = new CreateCompanyCommandHandler(_context);
        var dto = new CreateCompanyDto("Minimal Sirket", null, null, null);
        var command = new CreateCompanyCommand(dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Name.Should().Be("Minimal Sirket");
        result.TaxNumber.Should().BeNull();
        result.TaxOffice.Should().BeNull();
        result.Address.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldPersistToDatabase()
    {
        var handler = new CreateCompanyCommandHandler(_context);
        var dto = new CreateCompanyDto("Persist Test", "111", "VD", "Adres");

        await handler.Handle(new CreateCompanyCommand(dto), CancellationToken.None);

        _context.Companies.Should().HaveCount(1);
    }
}
