using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record RefreshTokenCommand(RefreshTokenDto Dto) : IRequest<AuthResponseDto>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Dto.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenCommandHandler(
    IApplicationDbContext context,
    ITokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.RefreshToken == request.Dto.RefreshToken
                && u.RefreshTokenExpiry > DateTime.UtcNow
                && u.IsActive, cancellationToken)
            ?? throw new UnauthorizedAccessException("Gecersiz veya suresi dolmus refresh token");

        var claims = await context.UserOperationClaims
            .Where(uoc => uoc.UserId == user.Id)
            .Include(uoc => uoc.OperationClaim)
            .Select(uoc => uoc.OperationClaim)
            .ToListAsync(cancellationToken);

        var token = tokenService.CreateToken(user, claims);
        var newRefreshToken = Guid.NewGuid().ToString();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            token, newRefreshToken, DateTime.UtcNow.AddHours(1),
            user.Id, user.Name, user.Email,
            claims.Select(c => c.Name).ToList());
    }
}
