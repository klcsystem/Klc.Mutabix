using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.MailParameters.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;

namespace Klc.Mutabix.Application.MailParameters.Commands;

public record CreateMailParameterCommand(CreateMailParameterDto Dto) : IRequest<MailParameterDto>;

public class CreateMailParameterCommandValidator : AbstractValidator<CreateMailParameterCommand>
{
    public CreateMailParameterCommandValidator()
    {
        RuleFor(x => x.Dto.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.SmtpServer).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.SmtpPort).InclusiveBetween(1, 65535);
        RuleFor(x => x.Dto.SenderEmail).NotEmpty().MaximumLength(200).EmailAddress();
        RuleFor(x => x.Dto.Password).NotEmpty().MaximumLength(500);
    }
}

public class CreateMailParameterCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateMailParameterCommand, MailParameterDto>
{
    public async Task<MailParameterDto> Handle(
        CreateMailParameterCommand request, CancellationToken cancellationToken)
    {
        var entity = new MailParameter
        {
            CompanyId = request.Dto.CompanyId,
            SmtpServer = request.Dto.SmtpServer,
            SmtpPort = request.Dto.SmtpPort,
            SenderEmail = request.Dto.SenderEmail,
            Password = request.Dto.Password,
            UseSsl = request.Dto.UseSsl
        };

        context.MailParameters.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new MailParameterDto(
            entity.Id, entity.CompanyId, entity.SmtpServer, entity.SmtpPort,
            entity.SenderEmail, entity.Password, entity.UseSsl,
            entity.IsActive, entity.CreatedAt);
    }
}
