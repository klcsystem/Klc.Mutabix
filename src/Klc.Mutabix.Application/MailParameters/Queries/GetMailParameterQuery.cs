using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.MailParameters.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.MailParameters.Queries;

public record GetMailParameterByCompanyQuery(int CompanyId) : IRequest<MailParameterDto?>;

public class GetMailParameterByCompanyQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMailParameterByCompanyQuery, MailParameterDto?>
{
    public async Task<MailParameterDto?> Handle(
        GetMailParameterByCompanyQuery request, CancellationToken cancellationToken)
    {
        return await context.MailParameters
            .Where(mp => mp.CompanyId == request.CompanyId && mp.IsActive)
            .Select(mp => new MailParameterDto(
                mp.Id, mp.CompanyId, mp.SmtpServer, mp.SmtpPort,
                mp.SenderEmail, mp.Password, mp.UseSsl,
                mp.IsActive, mp.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
