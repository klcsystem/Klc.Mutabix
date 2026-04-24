namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class ErpConnection : BaseAuditableEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ErpProviderType Provider { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ConnectionString { get; set; }
    public string? ApiUrl { get; set; }
    public string? ApiKey { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ExtraSettings { get; set; }
    public DateTime? LastSyncAt { get; set; }

    public ICollection<ErpSyncLog> SyncLogs { get; set; } = [];
}
