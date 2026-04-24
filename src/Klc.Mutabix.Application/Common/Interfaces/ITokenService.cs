using Klc.Mutabix.Domain.Entities;

namespace Klc.Mutabix.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, List<OperationClaim> operationClaims);
}
