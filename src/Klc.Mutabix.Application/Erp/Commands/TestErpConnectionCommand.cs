using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Erp.Commands;

public record TestErpConnectionCommand(int ConnectionId) : IRequest<bool>;

public class TestErpConnectionCommandHandler(
    IApplicationDbContext context,
    IErpAdapterFactory adapterFactory)
    : IRequestHandler<TestErpConnectionCommand, bool>
{
    public async Task<bool> Handle(TestErpConnectionCommand request, CancellationToken cancellationToken)
    {
        var connection = await context.ErpConnections
            .FirstOrDefaultAsync(c => c.Id == request.ConnectionId, cancellationToken)
            ?? throw new KeyNotFoundException("ERP baglantisi bulunamadi");

        var adapter = adapterFactory.GetAdapter(connection.Provider);
        return await adapter.TestConnectionAsync(connection, cancellationToken);
    }
}
