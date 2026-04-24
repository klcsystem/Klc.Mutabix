using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Queries;

public record GetAccountReconciliationsQuery(int CompanyId) : IRequest<List<AccountReconciliationDto>>;

public class GetAccountReconciliationsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAccountReconciliationsQuery, List<AccountReconciliationDto>>
{
    public async Task<List<AccountReconciliationDto>> Handle(
        GetAccountReconciliationsQuery request, CancellationToken cancellationToken)
    {
        return await context.AccountReconciliations
            .Include(ar => ar.CurrencyAccount)
            .Where(ar => ar.CompanyId == request.CompanyId && ar.IsActive)
            .OrderByDescending(ar => ar.CreatedAt)
            .Select(ar => new AccountReconciliationDto(
                ar.Id, ar.CompanyId, ar.CurrencyAccountId,
                ar.CurrencyAccount.Name,
                ar.StartDate, ar.EndDate, ar.CurrencyType,
                ar.DebitAmount, ar.CreditAmount,
                ar.Status, ar.Guid, ar.IsSent, ar.SentDate, ar.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
