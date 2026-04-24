using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Queries;

public record GetReconciliationStatsQuery(int CompanyId) : IRequest<ReconciliationStatsDto>;

public class GetReconciliationStatsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetReconciliationStatsQuery, ReconciliationStatsDto>
{
    public async Task<ReconciliationStatsDto> Handle(
        GetReconciliationStatsQuery request, CancellationToken cancellationToken)
    {
        var accountRecs = await context.AccountReconciliations
            .Where(ar => ar.CompanyId == request.CompanyId && ar.IsActive)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Pending = g.Count(x => x.Status == ReconciliationStatus.Pending),
                Approved = g.Count(x => x.Status == ReconciliationStatus.Approved),
                Rejected = g.Count(x => x.Status == ReconciliationStatus.Rejected)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var babsRecs = await context.BaBsReconciliations
            .Where(br => br.CompanyId == request.CompanyId && br.IsActive)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Pending = g.Count(x => x.Status == ReconciliationStatus.Pending),
                Approved = g.Count(x => x.Status == ReconciliationStatus.Approved),
                Rejected = g.Count(x => x.Status == ReconciliationStatus.Rejected)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new ReconciliationStatsDto(
            accountRecs?.Total ?? 0,
            accountRecs?.Pending ?? 0,
            accountRecs?.Approved ?? 0,
            accountRecs?.Rejected ?? 0,
            babsRecs?.Total ?? 0,
            babsRecs?.Pending ?? 0,
            babsRecs?.Approved ?? 0,
            babsRecs?.Rejected ?? 0);
    }
}
