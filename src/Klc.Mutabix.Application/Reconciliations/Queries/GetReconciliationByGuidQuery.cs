using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Queries;

public record ReconciliationPublicDto(
    string Guid,
    string Type,
    string SenderCompanyName,
    string? SenderTaxNumber,
    string? SenderAddress,
    string ReceiverAccountName,
    string? ReceiverTaxNumber,
    string Period,
    string CurrencyType,
    decimal DebitAmount,
    decimal CreditAmount,
    decimal Balance,
    string Status,
    List<ReconciliationDetailLineDto> Lines);

public record ReconciliationDetailLineDto(
    DateTime Date,
    string Description,
    decimal DebitAmount,
    decimal CreditAmount);

public record GetReconciliationByGuidQuery(string Guid) : IRequest<ReconciliationPublicDto?>;

public class GetReconciliationByGuidQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetReconciliationByGuidQuery, ReconciliationPublicDto?>
{
    public async Task<ReconciliationPublicDto?> Handle(
        GetReconciliationByGuidQuery request, CancellationToken cancellationToken)
    {
        // Try account reconciliation
        var accountRec = await context.AccountReconciliations
            .Include(r => r.CurrencyAccount)
                .ThenInclude(ca => ca.Company)
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.Guid == request.Guid && r.IsActive, cancellationToken);

        if (accountRec is not null)
        {
            var company = accountRec.CurrencyAccount.Company;
            var balance = accountRec.DebitAmount - accountRec.CreditAmount;
            var lines = accountRec.Details
                .OrderBy(d => d.TransactionDate)
                .Select(d => new ReconciliationDetailLineDto(
                    d.TransactionDate, d.Description, d.DebitAmount, d.CreditAmount))
                .ToList();

            return new ReconciliationPublicDto(
                accountRec.Guid!,
                "AccountReconciliation",
                company.Name,
                company.TaxNumber,
                company.Address,
                accountRec.CurrencyAccount.Name,
                accountRec.CurrencyAccount.TaxNumber,
                $"{accountRec.StartDate:dd.MM.yyyy} — {accountRec.EndDate:dd.MM.yyyy}",
                accountRec.CurrencyType.ToString(),
                accountRec.DebitAmount,
                accountRec.CreditAmount,
                balance,
                accountRec.Status.ToString(),
                lines);
        }

        // Try BaBs reconciliation
        var babsRec = await context.BaBsReconciliations
            .Include(r => r.CurrencyAccount)
                .ThenInclude(ca => ca.Company)
            .FirstOrDefaultAsync(r => r.Guid == request.Guid && r.IsActive, cancellationToken);

        if (babsRec is not null)
        {
            var company = babsRec.CurrencyAccount.Company;
            return new ReconciliationPublicDto(
                babsRec.Guid!,
                "BaBsReconciliation",
                company.Name,
                company.TaxNumber,
                company.Address,
                babsRec.CurrencyAccount.Name,
                babsRec.CurrencyAccount.TaxNumber,
                $"{babsRec.Year}/{babsRec.Month:D2}",
                "TRY",
                babsRec.Amount,
                0,
                babsRec.Amount,
                babsRec.Status.ToString(),
                []);
        }

        return null;
    }
}
