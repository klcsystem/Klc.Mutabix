using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Users.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Users.Commands;

public record CreateUserCommand(CreateUserDto Dto) : IRequest<UserDto>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}

public class CreateUserCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Users
            .AnyAsync(u => u.Email == request.Dto.Email, cancellationToken);
        if (exists)
            throw new InvalidOperationException("Bu e-posta adresi zaten kayitli");

        passwordService.CreatePasswordHash(request.Dto.Password,
            out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Name = request.Dto.Name,
            Email = request.Dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var roleNames = new List<string>();

        if (request.Dto.RoleIds is { Count: > 0 })
        {
            foreach (var roleId in request.Dto.RoleIds)
            {
                context.UserOperationClaims.Add(new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = roleId
                });
            }

            roleNames = await context.OperationClaims
                .Where(oc => request.Dto.RoleIds.Contains(oc.Id))
                .Select(oc => oc.Name)
                .ToListAsync(cancellationToken);
        }

        var companyIds = new List<int>();
        if (request.Dto.CompanyId.HasValue)
        {
            context.UserCompanies.Add(new UserCompany
            {
                UserId = user.Id,
                CompanyId = request.Dto.CompanyId.Value
            });
            companyIds.Add(request.Dto.CompanyId.Value);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new UserDto(user.Id, user.Name, user.Email, user.IsActive,
            user.CreatedAt, roleNames, companyIds);
    }
}
