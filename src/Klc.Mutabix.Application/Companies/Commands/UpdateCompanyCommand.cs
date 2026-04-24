using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Companies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Companies.Commands;

public record UpdateCompanyCommand(int Id, UpdateCompanyDto Dto) : IRequest<CompanyDto?>;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.TaxNumber).MaximumLength(20);
        RuleFor(x => x.Dto.TaxOffice).MaximumLength(100);
        RuleFor(x => x.Dto.Address).MaximumLength(500);
    }
}

public class UpdateCompanyCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateCompanyCommand, CompanyDto?>
{
    public async Task<CompanyDto?> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        entity.Name = request.Dto.Name;
        entity.TaxNumber = request.Dto.TaxNumber;
        entity.TaxOffice = request.Dto.TaxOffice;
        entity.Address = request.Dto.Address;
        entity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return new CompanyDto(
            entity.Id, entity.Name, entity.TaxNumber, entity.TaxOffice,
            entity.Address, entity.IsActive, entity.CreatedAt);
    }
}
