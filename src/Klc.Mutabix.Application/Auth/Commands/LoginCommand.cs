using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record LoginCommand(LoginDto Dto) : IRequest<AuthResponseDto>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6);
    }
}

public class LoginCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Dto.Email && u.IsActive, cancellationToken)
            ?? throw new UnauthorizedAccessException("Gecersiz e-posta veya sifre");

        if (!passwordService.VerifyPasswordHash(request.Dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new UnauthorizedAccessException("Gecersiz e-posta veya sifre");

        var claims = await context.UserOperationClaims
            .Where(uoc => uoc.UserId == user.Id)
            .Include(uoc => uoc.OperationClaim)
            .Select(uoc => uoc.OperationClaim)
            .ToListAsync(cancellationToken);

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
