using FluentAssertions;
using Klc.Mutabix.Application.Companies.Commands;
using Klc.Mutabix.Application.Companies.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Companies;

public class UpdateCompanyCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public UpdateCompanyCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenCompanyExists_ShouldUpdateAndReturnDto()
    {
        _context.Companies.Add(new Company { Name = "Eski Isim", IsActive = true });
        await _context.SaveChangesAsync();

        var handler = new UpdateCompanyCommandHandler(_context);
        var entity = await _context.Companies.FirstAsync();
        var dto = new UpdateCompanyDto("Yeni Isim", "9999999999", "Besiktas", "Ankara");
        var command = new UpdateCompanyCommand(entity.Id, dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Yeni Isim");
        result.TaxNumber.Should().Be("9999999999");
        result.TaxOffice.Should().Be("Besiktas");
        result.Address.Should().Be("Ankara");

        var updated = await _context.Companies.FirstAsync();
        updated.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenCompanyNotFound_ShouldReturnNull()
    {
        var handler = new UpdateCompanyCommandHandler(_context);
        var dto = new UpdateCompanyDto("Isim", null, null, null);
        var command = new UpdateCompanyCommand(999, dto);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();
    }
}
