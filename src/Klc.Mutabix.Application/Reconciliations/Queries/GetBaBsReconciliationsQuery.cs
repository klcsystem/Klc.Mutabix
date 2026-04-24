using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Queries;

public record GetBaBsReconciliationsQuery(int CompanyId) : IRequest<List<BaBsReconciliationDto>>;

public class GetBaBsReconciliationsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBaBsReconciliationsQuery, List<BaBsReconciliationDto>>
{
    public async Task<List<BaBsReconciliationDto>> Handle(
        GetBaBsReconciliationsQuery request, CancellationToken cancellationToken)
    {
        return await context.BaBsReconciliations
            .Include(br => br.CurrencyAccount)
            .Where(br => br.CompanyId == request.CompanyId && br.IsActive)
            .OrderByDescending(br => br.CreatedAt)
            .Select(br => new BaBsReconciliationDto(
                br.Id, br.CompanyId, br.CurrencyAccountId,
                br.CurrencyAccount.Name,
                br.Type, br.Year, br.Month, br.Amount, br.Quantity,
                br.Status, br.Guid, br.IsSent, br.SentDate, br.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
