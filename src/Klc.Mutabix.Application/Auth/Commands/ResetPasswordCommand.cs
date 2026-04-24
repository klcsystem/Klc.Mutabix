using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record ResetPasswordCommand(ResetPasswordDto Dto) : IRequest<bool>;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Dto.Token).NotEmpty();
        RuleFor(x => x.Dto.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}

public class ResetPasswordCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService)
    : IRequestHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.PasswordResetToken == request.Dto.Token
                && u.PasswordResetTokenExpiry > DateTime.UtcNow
                && u.IsActive, cancellationToken)
            ?? throw new UnauthorizedAccessException("Gecersiz veya suresi dolmus sifirlama tokeni");

        passwordService.CreatePasswordHash(request.Dto.NewPassword,
            out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
