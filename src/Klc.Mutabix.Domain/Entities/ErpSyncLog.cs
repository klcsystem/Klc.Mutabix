namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class ErpSyncLog : BaseEntity
{
    public int ErpConnectionId { get; set; }
    public ErpConnection ErpConnection { get; set; } = null!;

    public SyncStatus Status { get; set; } = SyncStatus.Pending;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int RecordsProcessed { get; set; }
    public int RecordsFailed { get; set; }
    public string? ErrorMessage { get; set; }
}
