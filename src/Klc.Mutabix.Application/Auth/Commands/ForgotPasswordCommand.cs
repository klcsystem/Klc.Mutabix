using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record ForgotPasswordCommand(ForgotPasswordDto Dto) : IRequest<bool>;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
    }
}

public class ForgotPasswordCommandHandler(
    IApplicationDbContext context,
    IMailService mailService)
    : IRequestHandler<ForgotPasswordCommand, bool>
{
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Dto.Email && u.IsActive, cancellationToken);

        // Always return true to prevent email enumeration
        if (user is null) return true;

        var resetToken = Guid.NewGuid().ToString();
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(2);
        await context.SaveChangesAsync(cancellationToken);

        var subject = "Mutabix - Sifre Sifirlama";
        var body = $"""
            Merhaba {user.Name},

            Sifre sifirlama talebiniz alinmistir.

            Sifirlama linki: /auth/reset-password?token={resetToken}

            Bu link 2 saat gecerlidir. Eger bu talebi siz yapmadiyseniz, bu mesaji dikkate almayin.

            Saygilarimizla,
            Mutabix
            """;

        await mailService.SendMailAsync(user.Email, subject, body, cancellationToken);
        return true;
    }
}
