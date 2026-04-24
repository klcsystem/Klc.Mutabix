using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record AutoMatchResult(int Matched, int Unmatched, List<string> Details);

public record AutoMatchReconciliationsCommand(int CompanyId) : IRequest<AutoMatchResult>;

public class AutoMatchReconciliationsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AutoMatchReconciliationsCommand, AutoMatchResult>
{
    public async Task<AutoMatchResult> Handle(
        AutoMatchReconciliationsCommand request, CancellationToken cancellationToken)
    {
        var pendingRecs = await context.AccountReconciliations
            .Include(r => r.CurrencyAccount)
            .Where(r => r.CompanyId == request.CompanyId
                     && r.IsActive
                     && r.Status == ReconciliationStatus.Pending)
            .ToListAsync(cancellationToken);

        var matched = 0;
        var unmatched = 0;
        var details = new List<string>();

        foreach (var rec in pendingRecs)
        {
            // Auto-match: if debit == credit (balanced), auto-approve
            if (rec.DebitAmount == rec.CreditAmount && rec.DebitAmount > 0)
            {
                rec.Status = ReconciliationStatus.Approved;
                rec.UpdatedAt = DateTime.UtcNow;
                matched++;
                details.Add($"{rec.CurrencyAccount.Name}: Otomatik eslestirildi (Borc={rec.DebitAmount:N2}, Alacak={rec.CreditAmount:N2})");
            }
            else
            {
                unmatched++;
            }
        }

        if (matched > 0)
            await context.SaveChangesAsync(cancellationToken);

        return new AutoMatchResult(matched, unmatched, details);
    }
}
