namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class AccountReconciliation : BaseAuditableEntity
{
    public int CompanyId { get; set; }
    public int CurrencyAccountId { get; set; }
    public CurrencyAccount CurrencyAccount { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CurrencyType CurrencyType { get; set; } = CurrencyType.TRY;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }

    public ReconciliationStatus Status { get; set; } = ReconciliationStatus.Pending;
    public string? Guid { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentDate { get; set; }

    public ICollection<AccountReconciliationDetail> Details { get; set; } = [];
}
