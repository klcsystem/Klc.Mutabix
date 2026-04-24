using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Companies.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;

namespace Klc.Mutabix.Application.Companies.Commands;

public record CreateCompanyCommand(CreateCompanyDto Dto) : IRequest<CompanyDto>;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.TaxNumber).MaximumLength(20);
        RuleFor(x => x.Dto.TaxOffice).MaximumLength(100);
        RuleFor(x => x.Dto.Address).MaximumLength(500);
    }
}

public class CreateCompanyCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Company
        {
            Name = request.Dto.Name,
            TaxNumber = request.Dto.TaxNumber,
            TaxOffice = request.Dto.TaxOffice,
            Address = request.Dto.Address
        };

        context.Companies.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new CompanyDto(
            entity.Id, entity.Name, entity.TaxNumber, entity.TaxOffice,
            entity.Address, entity.IsActive, entity.CreatedAt);
    }
}
