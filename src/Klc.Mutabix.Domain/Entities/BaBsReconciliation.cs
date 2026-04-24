namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class BaBsReconciliation : BaseAuditableEntity
{
    public int CompanyId { get; set; }
    public int CurrencyAccountId { get; set; }
    public CurrencyAccount CurrencyAccount { get; set; } = null!;

    public BaBsType Type { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
    public int Quantity { get; set; }

    public ReconciliationStatus Status { get; set; } = ReconciliationStatus.Pending;
    public string? Guid { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentDate { get; set; }

    public ICollection<BaBsReconciliationDetail> Details { get; set; } = [];
}
