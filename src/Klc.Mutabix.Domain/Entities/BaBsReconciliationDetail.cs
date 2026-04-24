namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class BaBsReconciliationDetail : BaseEntity
{
    public int BaBsReconciliationId { get; set; }
    public BaBsReconciliation BaBsReconciliation { get; set; } = null!;

    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
    public int Quantity { get; set; }
}
