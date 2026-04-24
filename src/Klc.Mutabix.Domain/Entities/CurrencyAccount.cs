namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class CurrencyAccount : BaseAuditableEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? Email { get; set; }
    public CurrencyType CurrencyType { get; set; } = CurrencyType.TRY;

    public ICollection<AccountReconciliation> AccountReconciliations { get; set; } = [];
    public ICollection<BaBsReconciliation> BaBsReconciliations { get; set; } = [];
}
