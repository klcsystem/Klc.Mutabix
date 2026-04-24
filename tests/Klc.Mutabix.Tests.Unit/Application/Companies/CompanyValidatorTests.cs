using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.Companies.Commands;
using Klc.Mutabix.Application.Companies.Dtos;

namespace Klc.Mutabix.Tests.Unit.Application.Companies;

public class CompanyValidatorTests
{
    private readonly CreateCompanyCommandValidator _createValidator = new();
    private readonly UpdateCompanyCommandValidator _updateValidator = new();

    // --- CreateCompanyCommand Validation ---

    [Fact]
    public void CreateCompany_WithValidData_ShouldPass()
    {
        var dto = new CreateCompanyDto("Test Sirket", "1234567890", "Kadikoy", "Istanbul");
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateCompany_WithEmptyName_ShouldFail()
    {
        var dto = new CreateCompanyDto("", null, null, null);
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void CreateCompany_WithNameExceeding200Chars_ShouldFail()
    {
        var dto = new CreateCompanyDto(new string('A', 201), null, null, null);
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void CreateCompany_WithTaxNumberExceeding20Chars_ShouldFail()
    {
        var dto = new CreateCompanyDto("Test", new string('1', 21), null, null);
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.TaxNumber);
    }

    [Fact]
    public void CreateCompany_WithTaxOfficeExceeding100Chars_ShouldFail()
    {
        var dto = new CreateCompanyDto("Test", null, new string('X', 101), null);
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.TaxOffice);
    }

    [Fact]
    public void CreateCompany_WithAddressExceeding500Chars_ShouldFail()
    {
        var dto = new CreateCompanyDto("Test", null, null, new string('A', 501));
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Address);
    }

    [Fact]
    public void CreateCompany_WithNullOptionalFields_ShouldPass()
    {
        var dto = new CreateCompanyDto("Minimal", null, null, null);
        var command = new CreateCompanyCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- UpdateCompanyCommand Validation ---

    [Fact]
    public void UpdateCompany_WithValidData_ShouldPass()
    {
        var dto = new UpdateCompanyDto("Guncellenmis", "9876543210", "Besiktas", "Ankara");
        var command = new UpdateCompanyCommand(1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateCompany_WithIdZero_ShouldFail()
    {
        var dto = new UpdateCompanyDto("Test", null, null, null);
        var command = new UpdateCompanyCommand(0, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void UpdateCompany_WithNegativeId_ShouldFail()
    {
        var dto = new UpdateCompanyDto("Test", null, null, null);
        var command = new UpdateCompanyCommand(-1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void UpdateCompany_WithEmptyName_ShouldFail()
    {
        var dto = new UpdateCompanyDto("", null, null, null);
        var command = new UpdateCompanyCommand(1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }
}
