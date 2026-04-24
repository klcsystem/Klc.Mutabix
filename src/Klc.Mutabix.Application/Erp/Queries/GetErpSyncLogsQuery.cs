using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Erp.Queries;

public record GetErpSyncLogsQuery(int ConnectionId, int Take = 20) : IRequest<List<ErpSyncLogDto>>;

public class GetErpSyncLogsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetErpSyncLogsQuery, List<ErpSyncLogDto>>
{
    public async Task<List<ErpSyncLogDto>> Handle(
        GetErpSyncLogsQuery request, CancellationToken cancellationToken)
    {
        return await context.ErpSyncLogs
            .Where(l => l.ErpConnectionId == request.ConnectionId)
            .OrderByDescending(l => l.StartedAt)
            .Take(request.Take)
            .Select(l => new ErpSyncLogDto(
                l.Id, l.ErpConnectionId, l.Status, l.StartedAt,
                l.CompletedAt, l.RecordsProcessed, l.RecordsFailed, l.ErrorMessage))
            .ToListAsync(cancellationToken);
    }
}
