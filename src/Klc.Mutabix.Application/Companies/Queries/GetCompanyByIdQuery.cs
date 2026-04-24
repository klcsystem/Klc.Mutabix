using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Companies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Companies.Queries;

public record GetCompanyByIdQuery(int Id) : IRequest<CompanyDto?>;

public class GetCompanyByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCompanyByIdQuery, CompanyDto?>
{
    public async Task<CompanyDto?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Companies
            .Where(c => c.Id == request.Id)
            .Select(c => new CompanyDto(
                c.Id, c.Name, c.TaxNumber, c.TaxOffice, c.Address, c.IsActive, c.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
