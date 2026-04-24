using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.CurrencyAccounts.Queries;

public record GetCurrencyAccountByIdQuery(int Id) : IRequest<CurrencyAccountDto?>;

public class GetCurrencyAccountByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCurrencyAccountByIdQuery, CurrencyAccountDto?>
{
    public async Task<CurrencyAccountDto?> Handle(
        GetCurrencyAccountByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.CurrencyAccounts
            .Where(ca => ca.Id == request.Id)
            .Select(ca => new CurrencyAccountDto(
                ca.Id, ca.CompanyId, ca.Code, ca.Name, ca.TaxNumber,
                ca.Email, ca.CurrencyType, ca.IsActive, ca.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
