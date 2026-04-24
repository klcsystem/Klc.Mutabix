using FluentAssertions;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Tests.Unit.Domain;

public class EntityTests
{
    [Fact]
    public void Company_ShouldHaveDefaultValues()
    {
        var company = new Company();

        company.Id.Should().Be(0);
        company.Name.Should().BeEmpty();
        company.TaxNumber.Should().BeNull();
        company.TaxOffice.Should().BeNull();
        company.Address.Should().BeNull();
        company.IsActive.Should().BeTrue();
        company.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        company.UpdatedAt.Should().BeNull();
        company.CreatedBy.Should().BeNull();
        company.UpdatedBy.Should().BeNull();
        company.UserCompanies.Should().BeEmpty();
        company.CurrencyAccounts.Should().BeEmpty();
    }

    [Fact]
    public void Company_ShouldSetProperties()
    {
        var company = new Company
        {
            Id = 1,
            Name = "Test Sirket",
            TaxNumber = "1234567890",
            TaxOffice = "Kadikoy",
            Address = "Istanbul, Turkiye"
        };

        company.Id.Should().Be(1);
        company.Name.Should().Be("Test Sirket");
        company.TaxNumber.Should().Be("1234567890");
        company.TaxOffice.Should().Be("Kadikoy");
        company.Address.Should().Be("Istanbul, Turkiye");
    }

    [Fact]
    public void CurrencyAccount_ShouldHaveDefaultValues()
    {
        var account = new CurrencyAccount();

        account.Id.Should().Be(0);
        account.CompanyId.Should().Be(0);
        account.Code.Should().BeEmpty();
        account.Name.Should().BeEmpty();
        account.TaxNumber.Should().BeNull();
        account.Email.Should().BeNull();
        account.CurrencyType.Should().Be(CurrencyType.TRY);
        account.IsActive.Should().BeTrue();
        account.AccountReconciliations.Should().BeEmpty();
        account.BaBsReconciliations.Should().BeEmpty();
    }

    [Fact]
    public void CurrencyAccount_ShouldSetProperties()
    {
        var account = new CurrencyAccount
        {
            Id = 5,
            CompanyId = 1,
            Code = "120-01",
            Name = "Test Cari",
            TaxNumber = "9876543210",
            Email = "cari@test.com",
            CurrencyType = CurrencyType.USD
        };

        account.Id.Should().Be(5);
        account.CompanyId.Should().Be(1);
        account.Code.Should().Be("120-01");
        account.Name.Should().Be("Test Cari");
        account.TaxNumber.Should().Be("9876543210");
        account.Email.Should().Be("cari@test.com");
        account.CurrencyType.Should().Be(CurrencyType.USD);
    }

    [Fact]
    public void AccountReconciliation_ShouldHaveDefaultValues()
    {
        var recon = new AccountReconciliation();

        recon.Status.Should().Be(ReconciliationStatus.Pending);
        recon.CurrencyType.Should().Be(CurrencyType.TRY);
        recon.IsSent.Should().BeFalse();
        recon.SentDate.Should().BeNull();
        recon.Guid.Should().BeNull();
        recon.DebitAmount.Should().Be(0);
        recon.CreditAmount.Should().Be(0);
        recon.Details.Should().BeEmpty();
    }

    [Fact]
    public void BaBsReconciliation_ShouldHaveDefaultValues()
    {
        var babs = new BaBsReconciliation();

        babs.Status.Should().Be(ReconciliationStatus.Pending);
        babs.IsSent.Should().BeFalse();
        babs.SentDate.Should().BeNull();
        babs.Guid.Should().BeNull();
        babs.Amount.Should().Be(0);
        babs.Quantity.Should().Be(0);
        babs.Details.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldHaveDefaultValues()
    {
        var user = new User();

        user.Name.Should().BeEmpty();
        user.Email.Should().BeEmpty();
        user.PasswordHash.Should().BeEmpty();
        user.PasswordSalt.Should().BeEmpty();
        user.IsActive.Should().BeTrue();
        user.UserCompanies.Should().BeEmpty();
        user.UserOperationClaims.Should().BeEmpty();
    }

    [Fact]
    public void BaseEntity_IsActive_ShouldDefaultToTrue()
    {
        var company = new Company();
        company.IsActive.Should().BeTrue();

        company.IsActive = false;
        company.IsActive.Should().BeFalse();
    }

    [Fact]
    public void BaseAuditableEntity_ShouldTrackAuditFields()
    {
        var company = new Company
        {
            CreatedBy = 1,
            UpdatedBy = 2,
            UpdatedAt = DateTime.UtcNow
        };

        company.CreatedBy.Should().Be(1);
        company.UpdatedBy.Should().Be(2);
        company.UpdatedAt.Should().NotBeNull();
    }
}
