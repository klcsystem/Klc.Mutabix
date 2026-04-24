using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record RegisterCommand(RegisterDto Dto) : IRequest<AuthResponseDto>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}

public class RegisterCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService,
    ITokenService tokenService)
    : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

        // Assign default Viewer role
        var viewerClaim = await context.OperationClaims
            .FirstOrDefaultAsync(oc => oc.Name == "Company.Read", cancellationToken);

        if (viewerClaim is not null)
        {
            context.UserOperationClaims.Add(new UserOperationClaim
            {
                UserId = user.Id,
                OperationClaimId = viewerClaim.Id
            });
        }

        // Link to company if provided
        if (request.Dto.CompanyId.HasValue)
        {
            context.UserCompanies.Add(new UserCompany
            {
                UserId = user.Id,
                CompanyId = request.Dto.CompanyId.Value
            });
        }

        await context.SaveChangesAsync(cancellationToken);

        var claims = viewerClaim is not null ? [viewerClaim] : new List<OperationClaim>();
        var token = tokenService.CreateToken(user, claims);
        var refreshToken = Guid.NewGuid().ToString();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            token, refreshToken, DateTime.UtcNow.AddHours(1),
            user.Id, user.Name, user.Email,
            claims.Select(c => c.Name).ToList());
    }
}
