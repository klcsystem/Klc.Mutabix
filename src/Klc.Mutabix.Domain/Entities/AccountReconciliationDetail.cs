namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class AccountReconciliationDetail : BaseEntity
{
    public int AccountReconciliationId { get; set; }
    public AccountReconciliation AccountReconciliation { get; set; } = null!;

    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public CurrencyType CurrencyType { get; set; } = CurrencyType.TRY;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
}
