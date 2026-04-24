using FluentAssertions;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Tests.Unit.Domain;

public class ErpEntityTests
{
    [Fact]
    public void ErpConnection_ShouldHaveDefaultValues()
    {
        var conn = new ErpConnection();

        conn.Id.Should().Be(0);
        conn.CompanyId.Should().Be(0);
        conn.Name.Should().BeEmpty();
        conn.ConnectionString.Should().BeNull();
        conn.ApiUrl.Should().BeNull();
        conn.ApiKey.Should().BeNull();
        conn.Username.Should().BeNull();
        conn.Password.Should().BeNull();
        conn.ExtraSettings.Should().BeNull();
        conn.LastSyncAt.Should().BeNull();
        conn.IsActive.Should().BeTrue();
        conn.SyncLogs.Should().BeEmpty();
    }

    [Fact]
    public void ErpConnection_ShouldSetProperties()
    {
        var conn = new ErpConnection
        {
            Id = 1,
            CompanyId = 5,
            Provider = ErpProviderType.Sap,
            Name = "SAP Production",
            ConnectionString = "Server=sap.local",
            ApiUrl = "https://sap.local/api",
            ApiKey = "secret-key",
            Username = "admin",
            Password = "pass123",
            ExtraSettings = "{\"timeout\": 30}"
        };

        conn.Id.Should().Be(1);
        conn.CompanyId.Should().Be(5);
        conn.Provider.Should().Be(ErpProviderType.Sap);
        conn.Name.Should().Be("SAP Production");
        conn.ConnectionString.Should().Be("Server=sap.local");
        conn.ApiUrl.Should().Be("https://sap.local/api");
        conn.ApiKey.Should().Be("secret-key");
        conn.Username.Should().Be("admin");
        conn.Password.Should().Be("pass123");
        conn.ExtraSettings.Should().Be("{\"timeout\": 30}");
    }

    [Fact]
    public void ErpSyncLog_ShouldHaveDefaultValues()
    {
        var log = new ErpSyncLog();

        log.Id.Should().Be(0);
        log.ErpConnectionId.Should().Be(0);
        log.Status.Should().Be(SyncStatus.Pending);
        log.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        log.CompletedAt.Should().BeNull();
        log.RecordsProcessed.Should().Be(0);
        log.RecordsFailed.Should().Be(0);
        log.ErrorMessage.Should().BeNull();
        log.IsActive.Should().BeTrue();
    }

    [Fact]
    public void ErpSyncLog_ShouldSetProperties()
    {
        var completedAt = DateTime.UtcNow;
        var log = new ErpSyncLog
        {
            Id = 10,
            ErpConnectionId = 3,
            Status = SyncStatus.Completed,
            RecordsProcessed = 150,
            RecordsFailed = 2,
            CompletedAt = completedAt,
            ErrorMessage = null
        };

        log.Id.Should().Be(10);
        log.ErpConnectionId.Should().Be(3);
        log.Status.Should().Be(SyncStatus.Completed);
        log.RecordsProcessed.Should().Be(150);
        log.RecordsFailed.Should().Be(2);
        log.CompletedAt.Should().Be(completedAt);
    }

    [Fact]
    public void ErpSyncLog_FailedStatus_ShouldHaveErrorMessage()
    {
        var log = new ErpSyncLog
        {
            Status = SyncStatus.Failed,
            ErrorMessage = "Connection timeout"
        };

        log.Status.Should().Be(SyncStatus.Failed);
        log.ErrorMessage.Should().Be("Connection timeout");
    }

    [Theory]
    [InlineData(ErpProviderType.Sap, 1)]
    [InlineData(ErpProviderType.Logo, 2)]
    [InlineData(ErpProviderType.Netsis, 3)]
    [InlineData(ErpProviderType.Parasut, 4)]
    [InlineData(ErpProviderType.Excel, 5)]
    [InlineData(ErpProviderType.Generic, 6)]
    public void ErpProviderType_ShouldHaveCorrectValues(ErpProviderType type, int expected)
    {
        ((int)type).Should().Be(expected);
    }

    [Fact]
    public void ErpProviderType_ShouldHave6Members()
    {
        Enum.GetValues<ErpProviderType>().Should().HaveCount(6);
    }

    [Theory]
    [InlineData(SyncStatus.Pending, 0)]
    [InlineData(SyncStatus.Running, 1)]
    [InlineData(SyncStatus.Completed, 2)]
    [InlineData(SyncStatus.Failed, 3)]
    public void SyncStatus_ShouldHaveCorrectValues(SyncStatus status, int expected)
    {
        ((int)status).Should().Be(expected);
    }

    [Fact]
    public void SyncStatus_ShouldHave4Members()
    {
        Enum.GetValues<SyncStatus>().Should().HaveCount(4);
    }
}
