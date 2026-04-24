namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class Company : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public string? Address { get; set; }

    public ICollection<UserCompany> UserCompanies { get; set; } = [];
    public ICollection<CurrencyAccount> CurrencyAccounts { get; set; } = [];
}
