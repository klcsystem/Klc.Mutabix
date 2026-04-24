using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;

public class GetUserByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(u => u.UserOperationClaims).ThenInclude(uoc => uoc.OperationClaim)
            .Include(u => u.UserCompanies)
            .Where(u => u.Id == request.Id)
            .Select(u => new UserDto(
                u.Id, u.Name, u.Email, u.IsActive, u.CreatedAt,
                u.UserOperationClaims.Select(uoc => uoc.OperationClaim.Name).ToList(),
                u.UserCompanies.Select(uc => uc.CompanyId).ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
