using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Erp.Queries;

public record GetErpConnectionsQuery(int CompanyId) : IRequest<List<ErpConnectionDto>>;

public class GetErpConnectionsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetErpConnectionsQuery, List<ErpConnectionDto>>
{
    public async Task<List<ErpConnectionDto>> Handle(
        GetErpConnectionsQuery request, CancellationToken cancellationToken)
    {
        return await context.ErpConnections
            .Where(e => e.CompanyId == request.CompanyId && e.IsActive)
            .OrderBy(e => e.Name)
            .Select(e => new ErpConnectionDto(
                e.Id, e.CompanyId, e.Provider, e.Name, e.ApiUrl,
                !string.IsNullOrEmpty(e.ApiKey), e.Username,
                e.LastSyncAt, e.IsActive, e.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
