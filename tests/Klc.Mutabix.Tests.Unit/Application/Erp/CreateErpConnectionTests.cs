using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.Erp.Commands;
using Klc.Mutabix.Application.Erp.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Erp;

public class CreateErpConnectionTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly CreateErpConnectionCommandHandler _handler;
    private readonly CreateErpConnectionCommandValidator _validator = new();
    private int _companyId;

    public CreateErpConnectionTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _handler = new CreateErpConnectionCommandHandler(_context);

        var company = new Company { Name = "ERP Test Sirket" };
        _context.Companies.Add(company);
        _context.SaveChanges();
        _companyId = company.Id;
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_ShouldCreateConnectionAndReturnDto()
    {
        var dto = new CreateErpConnectionDto(
            _companyId, ErpProviderType.Sap, "SAP Prod",
            null, "https://sap.local/api", "api-key-123",
            "admin", "pass", null);
        var command = new CreateErpConnectionCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CompanyId.Should().Be(_companyId);
        result.Provider.Should().Be(ErpProviderType.Sap);
        result.Name.Should().Be("SAP Prod");
        result.ApiUrl.Should().Be("https://sap.local/api");
        result.HasApiKey.Should().BeTrue();
        result.Username.Should().Be("admin");
        result.LastSyncAt.Should().BeNull();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNoApiKey_HasApiKeyShouldBeFalse()
    {
        var dto = new CreateErpConnectionDto(
            _companyId, ErpProviderType.Excel, "Excel Import",
            null, null, null, null, null, null);

        var result = await _handler.Handle(new CreateErpConnectionCommand(dto), CancellationToken.None);

        result.HasApiKey.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldPersistToDatabase()
    {
        var dto = new CreateErpConnectionDto(
            _companyId, ErpProviderType.Logo, "Logo Tiger",
            "Server=logo;Database=TIGERDB", null, null, "logouser", "logopass", null);

        await _handler.Handle(new CreateErpConnectionCommand(dto), CancellationToken.None);

        var saved = await _context.ErpConnections.FirstAsync();
        saved.Provider.Should().Be(ErpProviderType.Logo);
        saved.ConnectionString.Should().Be("Server=logo;Database=TIGERDB");
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        var dto = new CreateErpConnectionDto(1, ErpProviderType.Sap, "", null, null, null, null, null, null);
        var command = new CreateErpConnectionCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Validate_WithNameExceeding200Chars_ShouldFail()
    {
        var dto = new CreateErpConnectionDto(1, ErpProviderType.Sap, new string('X', 201), null, null, null, null, null, null);
        var command = new CreateErpConnectionCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Validate_WithCompanyIdZero_ShouldFail()
    {
        var dto = new CreateErpConnectionDto(0, ErpProviderType.Sap, "Test", null, null, null, null, null, null);
        var command = new CreateErpConnectionCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CompanyId);
    }

    [Fact]
    public void Validate_WithInvalidProvider_ShouldFail()
    {
        var dto = new CreateErpConnectionDto(1, (ErpProviderType)99, "Test", null, null, null, null, null, null);
        var command = new CreateErpConnectionCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Provider);
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateErpConnectionDto(1, ErpProviderType.Netsis, "Netsis Prod", "ConnStr", null, null, null, null, null);
        var command = new CreateErpConnectionCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
