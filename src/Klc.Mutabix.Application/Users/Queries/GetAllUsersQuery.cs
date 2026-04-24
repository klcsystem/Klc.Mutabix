using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserDto>>;

public class GetAllUsersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(u => u.UserOperationClaims).ThenInclude(uoc => uoc.OperationClaim)
            .Include(u => u.UserCompanies)
            .OrderBy(u => u.Name)
            .Select(u => new UserDto(
                u.Id, u.Name, u.Email, u.IsActive, u.CreatedAt,
                u.UserOperationClaims.Select(uoc => uoc.OperationClaim.Name).ToList(),
                u.UserCompanies.Select(uc => uc.CompanyId).ToList()))
            .ToListAsync(cancellationToken);
    }
}
