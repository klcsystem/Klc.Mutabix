namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class AuditLog : BaseEntity
{
    public string EntityName { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public int? UserId { get; set; }
    public string? UserEmail { get; set; }
}
