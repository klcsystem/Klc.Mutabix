using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Users.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Users.Commands;

public record UpdateUserCommand(int Id, UpdateUserDto Dto) : IRequest<UserDto?>;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(200);
    }
}

public class UpdateUserCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserOperationClaims)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null) return null;

        user.Name = request.Dto.Name;
        user.Email = request.Dto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        // Update roles if provided
        if (request.Dto.RoleIds is not null)
        {
            // Remove existing claims
            var existingClaims = await context.UserOperationClaims
                .Where(uoc => uoc.UserId == user.Id)
                .ToListAsync(cancellationToken);

            foreach (var claim in existingClaims)
                context.UserOperationClaims.Remove(claim);

            // Add new claims
            foreach (var roleId in request.Dto.RoleIds)
            {
                context.UserOperationClaims.Add(new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = roleId
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        var roleNames = await context.UserOperationClaims
            .Where(uoc => uoc.UserId == user.Id)
            .Include(uoc => uoc.OperationClaim)
            .Select(uoc => uoc.OperationClaim.Name)
            .ToListAsync(cancellationToken);

        var companyIds = await context.UserCompanies
            .Where(uc => uc.UserId == user.Id)
            .Select(uc => uc.CompanyId)
            .ToListAsync(cancellationToken);

        return new UserDto(user.Id, user.Name, user.Email, user.IsActive,
            user.CreatedAt, roleNames, companyIds);
    }
}
