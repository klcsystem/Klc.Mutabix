using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.MailParameters.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.MailParameters.Commands;

public record UpdateMailParameterCommand(int Id, UpdateMailParameterDto Dto) : IRequest<MailParameterDto?>;

public class UpdateMailParameterCommandValidator : AbstractValidator<UpdateMailParameterCommand>
{
    public UpdateMailParameterCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SmtpServer).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.SmtpPort).InclusiveBetween(1, 65535);
        RuleFor(x => x.Dto.SenderEmail).NotEmpty().MaximumLength(200).EmailAddress();
        RuleFor(x => x.Dto.Password).NotEmpty().MaximumLength(500);
    }
}

public class UpdateMailParameterCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateMailParameterCommand, MailParameterDto?>
{
    public async Task<MailParameterDto?> Handle(
        UpdateMailParameterCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.MailParameters
            .FirstOrDefaultAsync(mp => mp.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        entity.SmtpServer = request.Dto.SmtpServer;
        entity.SmtpPort = request.Dto.SmtpPort;
        entity.SenderEmail = request.Dto.SenderEmail;
        entity.Password = request.Dto.Password;
        entity.UseSsl = request.Dto.UseSsl;
        entity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return new MailParameterDto(
            entity.Id, entity.CompanyId, entity.SmtpServer, entity.SmtpPort,
            entity.SenderEmail, entity.Password, entity.UseSsl,
            entity.IsActive, entity.CreatedAt);
    }
}
