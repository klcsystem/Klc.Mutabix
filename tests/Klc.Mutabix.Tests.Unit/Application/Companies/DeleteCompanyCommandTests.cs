using FluentAssertions;
using Klc.Mutabix.Application.Companies.Commands;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Companies;

public class DeleteCompanyCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;

    public DeleteCompanyCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenCompanyExists_ShouldSoftDeleteAndReturnTrue()
    {
        _context.Companies.Add(new Company { Name = "Silinecek", IsActive = true });
        await _context.SaveChangesAsync();
        var entity = await _context.Companies.FirstAsync();

        var handler = new DeleteCompanyCommandHandler(_context);
        var result = await handler.Handle(new DeleteCompanyCommand(entity.Id), CancellationToken.None);

        result.Should().BeTrue();
        var deleted = await _context.Companies.FirstAsync();
        deleted.IsActive.Should().BeFalse();
        deleted.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenCompanyNotFound_ShouldReturnFalse()
    {
        var handler = new DeleteCompanyCommandHandler(_context);
        var result = await handler.Handle(new DeleteCompanyCommand(999), CancellationToken.None);

        result.Should().BeFalse();
    }
}
