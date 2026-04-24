using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reports.Dtos;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reports.Queries;

public record GetReconciliationSummaryQuery(int CompanyId) : IRequest<ReconciliationSummaryDto>;

public class GetReconciliationSummaryQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetReconciliationSummaryQuery, ReconciliationSummaryDto>
{
    public async Task<ReconciliationSummaryDto> Handle(
        GetReconciliationSummaryQuery request, CancellationToken cancellationToken)
    {
        var recs = await context.AccountReconciliations
            .Where(r => r.CompanyId == request.CompanyId && r.IsActive)
            .ToListAsync(cancellationToken);

        return new ReconciliationSummaryDto(
            recs.Count,
            recs.Count(r => r.Status == ReconciliationStatus.Pending),
            recs.Count(r => r.Status == ReconciliationStatus.Sent),
            recs.Count(r => r.Status == ReconciliationStatus.Approved),
            recs.Count(r => r.Status == ReconciliationStatus.Rejected),
            recs.Sum(r => r.DebitAmount),
            recs.Sum(r => r.CreditAmount));
    }
}
