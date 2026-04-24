namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class OperationClaim : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<UserOperationClaim> UserOperationClaims { get; set; } = [];
}
