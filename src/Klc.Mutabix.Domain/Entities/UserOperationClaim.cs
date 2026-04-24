namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class UserOperationClaim : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int OperationClaimId { get; set; }
    public OperationClaim OperationClaim { get; set; } = null!;
}
