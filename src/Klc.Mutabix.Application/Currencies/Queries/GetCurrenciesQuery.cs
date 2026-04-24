using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Currencies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Currencies.Queries;

public record GetCurrenciesQuery : IRequest<List<CurrencyDto>>;

public class GetCurrenciesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCurrenciesQuery, List<CurrencyDto>>
{
    public async Task<List<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await context.Currencies
            .Where(c => c.IsActive)
            .OrderBy(c => c.Code)
            .Select(c => new CurrencyDto(c.Id, c.Code, c.Name, c.Symbol, c.CurrencyType))
            .ToListAsync(cancellationToken);
    }
}
