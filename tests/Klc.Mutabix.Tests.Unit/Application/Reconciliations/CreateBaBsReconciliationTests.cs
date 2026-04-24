using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.Reconciliations.Commands;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Tests.Unit.Application.Reconciliations;

public class CreateBaBsReconciliationTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly CreateBaBsReconciliationCommandHandler _handler;
    private readonly CreateBaBsReconciliationCommandValidator _validator = new();
    private int _companyId;
    private int _accountId;

    public CreateBaBsReconciliationTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _handler = new CreateBaBsReconciliationCommandHandler(_context);

        var company = new Company { Name = "BaBs Test" };
        _context.Companies.Add(company);
        _context.SaveChanges();
        _companyId = company.Id;

        var account = new CurrencyAccount
        {
            CompanyId = _companyId, Code = "320-01", Name = "BaBs Cari",
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
        var dto = new CreateBaBsReconciliationDto(
            _companyId, _accountId, BaBsType.Ba, 2026, 3, 50000m, 15);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(ReconciliationStatus.Pending);
        result.Guid.Should().NotBeNullOrEmpty();
        result.Type.Should().Be(BaBsType.Ba);
        result.Year.Should().Be(2026);
        result.Month.Should().Be(3);
        result.Amount.Should().Be(50000m);
        result.Quantity.Should().Be(15);
        result.CurrencyAccountName.Should().Be("BaBs Cari");
    }

    [Fact]
    public void Validate_WithInvalidMonth_ShouldFail()
    {
        var dto = new CreateBaBsReconciliationDto(1, 1, BaBsType.Ba, 2026, 13, 1000m, 1);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Month);
    }

    [Fact]
    public void Validate_WithMonthZero_ShouldFail()
    {
        var dto = new CreateBaBsReconciliationDto(1, 1, BaBsType.Bs, 2026, 0, 1000m, 1);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Month);
    }

    [Fact]
    public void Validate_WithYearOutOfRange_ShouldFail()
    {
        var dto = new CreateBaBsReconciliationDto(1, 1, BaBsType.Ba, 1999, 1, 1000m, 1);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Year);
    }

    [Fact]
    public void Validate_WithInvalidBaBsType_ShouldFail()
    {
        var dto = new CreateBaBsReconciliationDto(1, 1, (BaBsType)99, 2026, 1, 1000m, 1);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Type);
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateBaBsReconciliationDto(1, 1, BaBsType.Ba, 2026, 6, 50000m, 10);
        var command = new CreateBaBsReconciliationCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
