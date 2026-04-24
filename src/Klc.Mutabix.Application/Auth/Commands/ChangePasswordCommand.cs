using FluentValidation;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Auth.Commands;

public record ChangePasswordCommand(int UserId, ChangePasswordDto Dto) : IRequest<bool>;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Dto.CurrentPassword).NotEmpty();
        RuleFor(x => x.Dto.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}

public class ChangePasswordCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService)
    : IRequestHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.IsActive, cancellationToken)
            ?? throw new KeyNotFoundException("Kullanici bulunamadi");

        if (!passwordService.VerifyPasswordHash(request.Dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            throw new UnauthorizedAccessException("Mevcut sifre yanlis");

        passwordService.CreatePasswordHash(request.Dto.NewPassword,
            out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
