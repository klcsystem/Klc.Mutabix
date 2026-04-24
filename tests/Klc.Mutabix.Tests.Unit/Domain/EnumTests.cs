using FluentAssertions;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Tests.Unit.Domain;

public class EnumTests
{
    [Theory]
    [InlineData(ReconciliationStatus.Pending, 0)]
    [InlineData(ReconciliationStatus.Sent, 1)]
    [InlineData(ReconciliationStatus.Approved, 2)]
    [InlineData(ReconciliationStatus.Rejected, 3)]
    [InlineData(ReconciliationStatus.Expired, 4)]
    public void ReconciliationStatus_ShouldHaveCorrectValues(ReconciliationStatus status, int expected)
    {
        ((int)status).Should().Be(expected);
    }

    [Fact]
    public void ReconciliationStatus_ShouldHave5Members()
    {
        Enum.GetValues<ReconciliationStatus>().Should().HaveCount(5);
    }

    [Theory]
    [InlineData(CurrencyType.TRY, 1)]
    [InlineData(CurrencyType.USD, 2)]
    [InlineData(CurrencyType.EUR, 3)]
    [InlineData(CurrencyType.GBP, 4)]
    public void CurrencyType_ShouldHaveCorrectValues(CurrencyType currency, int expected)
    {
        ((int)currency).Should().Be(expected);
    }

    [Fact]
    public void CurrencyType_ShouldHave4Members()
    {
        Enum.GetValues<CurrencyType>().Should().HaveCount(4);
    }

    [Theory]
    [InlineData(BaBsType.Ba, 1)]
    [InlineData(BaBsType.Bs, 2)]
    public void BaBsType_ShouldHaveCorrectValues(BaBsType type, int expected)
    {
        ((int)type).Should().Be(expected);
    }

    [Theory]
    [InlineData(ReconciliationType.AccountReconciliation, 1)]
    [InlineData(ReconciliationType.BaBsReconciliation, 2)]
    public void ReconciliationType_ShouldHaveCorrectValues(ReconciliationType type, int expected)
    {
        ((int)type).Should().Be(expected);
    }

    [Theory]
    [InlineData(ErpSourceType.Manual, 0)]
    [InlineData(ErpSourceType.Excel, 1)]
    [InlineData(ErpSourceType.Sap, 2)]
    [InlineData(ErpSourceType.Logo, 3)]
    [InlineData(ErpSourceType.Mikro, 4)]
    [InlineData(ErpSourceType.Netsis, 5)]
    public void ErpSourceType_ShouldHaveCorrectValues(ErpSourceType type, int expected)
    {
        ((int)type).Should().Be(expected);
    }

    [Fact]
    public void ErpSourceType_ShouldHave6Members()
    {
        Enum.GetValues<ErpSourceType>().Should().HaveCount(6);
    }

    [Theory]
    [InlineData(MailTemplateType.AccountReconciliation, 1)]
    [InlineData(MailTemplateType.BaBsReconciliation, 2)]
    [InlineData(MailTemplateType.Reminder, 3)]
    [InlineData(MailTemplateType.Approval, 4)]
    [InlineData(MailTemplateType.Rejection, 5)]
    public void MailTemplateType_ShouldHaveCorrectValues(MailTemplateType type, int expected)
    {
        ((int)type).Should().Be(expected);
    }

    [Fact]
    public void MailTemplateType_ShouldHave5Members()
    {
        Enum.GetValues<MailTemplateType>().Should().HaveCount(5);
    }
}
