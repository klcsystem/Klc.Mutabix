using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reports.Dtos;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reports.Queries;

public record GetMonthlyTrendQuery(int CompanyId, int Months = 12) : IRequest<List<MonthlyTrendDto>>;

public class GetMonthlyTrendQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMonthlyTrendQuery, List<MonthlyTrendDto>>
{
    public async Task<List<MonthlyTrendDto>> Handle(
        GetMonthlyTrendQuery request, CancellationToken cancellationToken)
    {
        var since = DateTime.UtcNow.AddMonths(-request.Months);

        var accountRecs = await context.AccountReconciliations
            .Where(r => r.CompanyId == request.CompanyId && r.IsActive && r.CreatedAt >= since)
            .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Count = g.Count(),
                Approved = g.Count(r => r.Status == ReconciliationStatus.Approved),
                Rejected = g.Count(r => r.Status == ReconciliationStatus.Rejected)
            })
            .ToListAsync(cancellationToken);

        var babsRecs = await context.BaBsReconciliations
            .Where(r => r.CompanyId == request.CompanyId && r.IsActive && r.CreatedAt >= since)
            .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var months = Enumerable.Range(0, request.Months)
            .Select(i => DateTime.UtcNow.AddMonths(-i))
            .Select(d => new { d.Year, d.Month })
            .Reverse();

        return months.Select(m =>
        {
            var acct = accountRecs.FirstOrDefault(a => a.Year == m.Year && a.Month == m.Month);
            var babs = babsRecs.FirstOrDefault(b => b.Year == m.Year && b.Month == m.Month);
            return new MonthlyTrendDto(
                m.Year, m.Month,
                acct?.Count ?? 0,
                babs?.Count ?? 0,
                acct?.Approved ?? 0,
                acct?.Rejected ?? 0);
        }).ToList();
    }
}
