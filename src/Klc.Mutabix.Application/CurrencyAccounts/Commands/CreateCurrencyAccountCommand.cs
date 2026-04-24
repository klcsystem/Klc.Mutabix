using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;

namespace Klc.Mutabix.Application.CurrencyAccounts.Commands;

public record CreateCurrencyAccountCommand(CreateCurrencyAccountDto Dto) : IRequest<CurrencyAccountDto>;

public class CreateCurrencyAccountCommandValidator : AbstractValidator<CreateCurrencyAccountCommand>
{
    public CreateCurrencyAccountCommandValidator()
    {
        RuleFor(x => x.Dto.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.TaxNumber).MaximumLength(20);
        RuleFor(x => x.Dto.Email).MaximumLength(200).EmailAddress().When(x => !string.IsNullOrEmpty(x.Dto.Email));
        RuleFor(x => x.Dto.CurrencyType).IsInEnum();
    }
}

public class CreateCurrencyAccountCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateCurrencyAccountCommand, CurrencyAccountDto>
{
    public async Task<CurrencyAccountDto> Handle(
        CreateCurrencyAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = new CurrencyAccount
        {
            CompanyId = request.Dto.CompanyId,
            Code = request.Dto.Code,
            Name = request.Dto.Name,
            TaxNumber = request.Dto.TaxNumber,
            Email = request.Dto.Email,
            CurrencyType = request.Dto.CurrencyType
        };

        context.CurrencyAccounts.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new CurrencyAccountDto(
            entity.Id, entity.CompanyId, entity.Code, entity.Name,
            entity.TaxNumber, entity.Email, entity.CurrencyType,
            entity.IsActive, entity.CreatedAt);
    }
}
