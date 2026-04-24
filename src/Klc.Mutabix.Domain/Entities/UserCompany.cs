namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class UserCompany : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}
