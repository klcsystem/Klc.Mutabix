using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record DeleteReconciliationCommand(int Id, ReconciliationType Type) : IRequest<bool>;

public class DeleteReconciliationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteReconciliationCommand, bool>
{
    public async Task<bool> Handle(DeleteReconciliationCommand request, CancellationToken cancellationToken)
    {
        if (request.Type == ReconciliationType.AccountReconciliation)
        {
            var entity = await context.AccountReconciliations
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var entity = await context.BaBsReconciliations
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity is null) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
