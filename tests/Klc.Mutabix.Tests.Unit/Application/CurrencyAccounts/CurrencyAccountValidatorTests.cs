using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.CurrencyAccounts.Commands;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Tests.Unit.Application.CurrencyAccounts;

public class CurrencyAccountValidatorTests
{
    private readonly CreateCurrencyAccountCommandValidator _createValidator = new();
    private readonly UpdateCurrencyAccountCommandValidator _updateValidator = new();

    // --- Create Validation ---

    [Fact]
    public void Create_WithValidData_ShouldPass()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test Cari", "1234567890", "test@test.com", CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Create_WithCompanyIdZero_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(0, "120-01", "Test", null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CompanyId);
    }

    [Fact]
    public void Create_WithEmptyCode_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "", "Test", null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Code);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "", null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Create_WithCodeExceeding50Chars_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, new string('X', 51), "Test", null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Code);
    }

    [Fact]
    public void Create_WithNameExceeding200Chars_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", new string('X', 201), null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", null, "gecersiz-email", CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Create_WithNullEmail_ShouldPass()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", null, null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Create_WithEmptyEmail_ShouldPass()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", null, "", CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Create_WithInvalidEnumValue_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", null, null, (CurrencyType)99);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CurrencyType);
    }

    [Fact]
    public void Create_WithTaxNumberExceeding20Chars_ShouldFail()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", new string('1', 21), null, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.TaxNumber);
    }

    [Fact]
    public void Create_WithEmailExceeding200Chars_ShouldFail()
    {
        var longEmail = new string('a', 192) + "@test.com";
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Test", null, longEmail, CurrencyType.TRY);
        var command = new CreateCurrencyAccountCommand(dto);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    // --- Update Validation ---

    [Fact]
    public void Update_WithValidData_ShouldPass()
    {
        var dto = new UpdateCurrencyAccountDto("120-01", "Test Cari", "1234567890", "test@test.com", CurrencyType.EUR);
        var command = new UpdateCurrencyAccountCommand(1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Update_WithIdZero_ShouldFail()
    {
        var dto = new UpdateCurrencyAccountDto("120-01", "Test", null, null, CurrencyType.TRY);
        var command = new UpdateCurrencyAccountCommand(0, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Update_WithEmptyName_ShouldFail()
    {
        var dto = new UpdateCurrencyAccountDto("120-01", "", null, null, CurrencyType.TRY);
        var command = new UpdateCurrencyAccountCommand(1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Update_WithInvalidEmail_ShouldFail()
    {
        var dto = new UpdateCurrencyAccountDto("120-01", "Test", null, "not-an-email", CurrencyType.TRY);
        var command = new UpdateCurrencyAccountCommand(1, dto);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }
}
