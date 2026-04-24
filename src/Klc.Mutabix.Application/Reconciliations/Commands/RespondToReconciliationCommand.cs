using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record RespondToReconciliationCommand(string Guid, RespondToReconciliationDto Dto) : IRequest<bool>;

public class RespondToReconciliationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RespondToReconciliationCommand, bool>
{
    public async Task<bool> Handle(
        RespondToReconciliationCommand request, CancellationToken cancellationToken)
    {
        var newStatus = request.Dto.IsApproved
            ? ReconciliationStatus.Approved
            : ReconciliationStatus.Rejected;

        // Try account reconciliation first
        var accountRec = await context.AccountReconciliations
            .FirstOrDefaultAsync(r => r.Guid == request.Guid, cancellationToken);

        if (accountRec is not null)
        {
            accountRec.Status = newStatus;
            accountRec.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Try BaBs reconciliation
        var babsRec = await context.BaBsReconciliations
            .FirstOrDefaultAsync(r => r.Guid == request.Guid, cancellationToken);

        if (babsRec is not null)
        {
            babsRec.Status = newStatus;
            babsRec.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}
