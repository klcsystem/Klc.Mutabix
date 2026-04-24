using FluentAssertions;
using Klc.Mutabix.Application.Companies.Dtos;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Tests.Unit.Application.Common;

public class DtoMappingTests
{
    [Fact]
    public void CompanyDto_ShouldBeCreatedWithAllFields()
    {
        var now = DateTime.UtcNow;
        var dto = new CompanyDto(1, "Test", "123", "Kadikoy", "Istanbul", true, now);

        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Test");
        dto.TaxNumber.Should().Be("123");
        dto.TaxOffice.Should().Be("Kadikoy");
        dto.Address.Should().Be("Istanbul");
        dto.IsActive.Should().BeTrue();
        dto.CreatedAt.Should().Be(now);
    }

    [Fact]
    public void CompanyDto_WithNullFields_ShouldBeValid()
    {
        var dto = new CompanyDto(1, "Test", null, null, null, true, DateTime.UtcNow);

        dto.TaxNumber.Should().BeNull();
        dto.TaxOffice.Should().BeNull();
        dto.Address.Should().BeNull();
    }

    [Fact]
    public void CreateCompanyDto_ShouldHoldValues()
    {
        var dto = new CreateCompanyDto("Sirket", "VKN", "VD", "Adres");

        dto.Name.Should().Be("Sirket");
        dto.TaxNumber.Should().Be("VKN");
        dto.TaxOffice.Should().Be("VD");
        dto.Address.Should().Be("Adres");
    }

    [Fact]
    public void UpdateCompanyDto_ShouldHoldValues()
    {
        var dto = new UpdateCompanyDto("Guncellenmis", "VKN2", "VD2", "Adres2");

        dto.Name.Should().Be("Guncellenmis");
        dto.TaxNumber.Should().Be("VKN2");
    }

    [Fact]
    public void CurrencyAccountDto_ShouldBeCreatedWithAllFields()
    {
        var now = DateTime.UtcNow;
        var dto = new CurrencyAccountDto(1, 2, "120-01", "Cari", "VKN", "e@t.com", CurrencyType.EUR, true, now);

        dto.Id.Should().Be(1);
        dto.CompanyId.Should().Be(2);
        dto.Code.Should().Be("120-01");
        dto.Name.Should().Be("Cari");
        dto.TaxNumber.Should().Be("VKN");
        dto.Email.Should().Be("e@t.com");
        dto.CurrencyType.Should().Be(CurrencyType.EUR);
        dto.IsActive.Should().BeTrue();
        dto.CreatedAt.Should().Be(now);
    }

    [Fact]
    public void CreateCurrencyAccountDto_ShouldHoldValues()
    {
        var dto = new CreateCurrencyAccountDto(1, "120-01", "Cari", "VKN", "e@t.com", CurrencyType.USD);

        dto.CompanyId.Should().Be(1);
        dto.Code.Should().Be("120-01");
        dto.CurrencyType.Should().Be(CurrencyType.USD);
    }

    [Fact]
    public void UpdateCurrencyAccountDto_ShouldHoldValues()
    {
        var dto = new UpdateCurrencyAccountDto("320-02", "Yeni Cari", null, null, CurrencyType.GBP);

        dto.Code.Should().Be("320-02");
        dto.Name.Should().Be("Yeni Cari");
        dto.CurrencyType.Should().Be(CurrencyType.GBP);
    }

    [Fact]
    public void CompanyDto_Equality_SameValues_ShouldBeEqual()
    {
        var now = DateTime.UtcNow;
        var dto1 = new CompanyDto(1, "Test", "123", "VD", "Adres", true, now);
        var dto2 = new CompanyDto(1, "Test", "123", "VD", "Adres", true, now);

        dto1.Should().Be(dto2);
    }

    [Fact]
    public void CompanyDto_Equality_DifferentValues_ShouldNotBeEqual()
    {
        var now = DateTime.UtcNow;
        var dto1 = new CompanyDto(1, "A", null, null, null, true, now);
        var dto2 = new CompanyDto(2, "B", null, null, null, true, now);

        dto1.Should().NotBe(dto2);
    }
}
