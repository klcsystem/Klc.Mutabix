using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Companies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Companies.Queries;

public record GetCompaniesQuery : IRequest<List<CompanyDto>>;

public class GetCompaniesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCompaniesQuery, List<CompanyDto>>
{
    public async Task<List<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await context.Companies
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CompanyDto(
                c.Id, c.Name, c.TaxNumber, c.TaxOffice, c.Address, c.IsActive, c.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
