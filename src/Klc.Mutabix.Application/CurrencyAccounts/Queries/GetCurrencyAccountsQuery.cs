using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.CurrencyAccounts.Queries;

public record GetCurrencyAccountsQuery(int CompanyId) : IRequest<List<CurrencyAccountDto>>;

public class GetCurrencyAccountsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCurrencyAccountsQuery, List<CurrencyAccountDto>>
{
    public async Task<List<CurrencyAccountDto>> Handle(
        GetCurrencyAccountsQuery request, CancellationToken cancellationToken)
    {
        return await context.CurrencyAccounts
            .Where(ca => ca.CompanyId == request.CompanyId && ca.IsActive)
            .OrderBy(ca => ca.Code)
            .Select(ca => new CurrencyAccountDto(
                ca.Id, ca.CompanyId, ca.Code, ca.Name, ca.TaxNumber,
                ca.Email, ca.CurrencyType, ca.IsActive, ca.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
