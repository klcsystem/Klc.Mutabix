using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.Reconciliations.Commands;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Reconciliations;

public class CreateAccountReconciliationTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly CreateAccountReconciliationCommandHandler _handler;
    private readonly CreateAccountReconciliationCommandValidator _validator = new();
    private int _companyId;
    private int _accountId;

    public CreateAccountReconciliationTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _handler = new CreateAccountReconciliationCommandHandler(_context);

        var company = new Company { Name = "Test Sirket" };
        _context.Companies.Add(company);
        _context.SaveChanges();
        _companyId = company.Id;

        var account = new CurrencyAccount
        {
            CompanyId = _companyId, Code = "120-01", Name = "Cari Hesap",
            CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        _context.SaveChanges();
        _accountId = account.Id;
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_ShouldCreateWithPendingStatusAndGuid()
    {
        var dto = new CreateAccountReconciliationDto(
            _companyId, _accountId,
            new DateTime(2026, 1, 1), new DateTime(2026, 3, 31),
            CurrencyType.TRY, 10000m, 8000m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(ReconciliationStatus.Pending);
        result.Guid.Should().NotBeNullOrEmpty();
        result.IsSent.Should().BeFalse();
        result.SentDate.Should().BeNull();
        result.DebitAmount.Should().Be(10000m);
        result.CreditAmount.Should().Be(8000m);
        result.CurrencyAccountName.Should().Be("Cari Hesap");
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_ShouldPersistToDatabase()
    {
        var dto = new CreateAccountReconciliationDto(
            _companyId, _accountId,
            new DateTime(2026, 1, 1), new DateTime(2026, 6, 30),
            CurrencyType.USD, 5000m, 3000m);

        await _handler.Handle(new CreateAccountReconciliationCommand(dto), CancellationToken.None);

        var saved = await _context.AccountReconciliations.FirstAsync();
        saved.CompanyId.Should().Be(_companyId);
        saved.CurrencyAccountId.Should().Be(_accountId);
        saved.Guid.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Validate_WithStartDateAfterEndDate_ShouldFail()
    {
        var dto = new CreateAccountReconciliationDto(
            1, 1,
            new DateTime(2026, 6, 30), new DateTime(2026, 1, 1),
            CurrencyType.TRY, 1000m, 500m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.StartDate);
    }

    [Fact]
    public void Validate_WithCompanyIdZero_ShouldFail()
    {
        var dto = new CreateAccountReconciliationDto(
            0, 1,
            new DateTime(2026, 1, 1), new DateTime(2026, 3, 31),
            CurrencyType.TRY, 1000m, 500m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CompanyId);
    }

    [Fact]
    public void Validate_WithCurrencyAccountIdZero_ShouldFail()
    {
        var dto = new CreateAccountReconciliationDto(
            1, 0,
            new DateTime(2026, 1, 1), new DateTime(2026, 3, 31),
            CurrencyType.TRY, 1000m, 500m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CurrencyAccountId);
    }

    [Fact]
    public void Validate_WithInvalidCurrencyType_ShouldFail()
    {
        var dto = new CreateAccountReconciliationDto(
            1, 1,
            new DateTime(2026, 1, 1), new DateTime(2026, 3, 31),
            (CurrencyType)99, 1000m, 500m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.CurrencyType);
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateAccountReconciliationDto(
            1, 1,
            new DateTime(2026, 1, 1), new DateTime(2026, 3, 31),
            CurrencyType.TRY, 10000m, 8000m);
        var command = new CreateAccountReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
