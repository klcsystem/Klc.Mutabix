using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.CurrencyAccounts.Commands;

public record UpdateCurrencyAccountCommand(int Id, UpdateCurrencyAccountDto Dto) : IRequest<CurrencyAccountDto?>;

public class UpdateCurrencyAccountCommandValidator : AbstractValidator<UpdateCurrencyAccountCommand>
{
    public UpdateCurrencyAccountCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.TaxNumber).MaximumLength(20);
        RuleFor(x => x.Dto.Email).MaximumLength(200).EmailAddress().When(x => !string.IsNullOrEmpty(x.Dto.Email));
        RuleFor(x => x.Dto.CurrencyType).IsInEnum();
    }
}

public class UpdateCurrencyAccountCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateCurrencyAccountCommand, CurrencyAccountDto?>
{
    public async Task<CurrencyAccountDto?> Handle(
        UpdateCurrencyAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.CurrencyAccounts
            .FirstOrDefaultAsync(ca => ca.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        entity.Code = request.Dto.Code;
        entity.Name = request.Dto.Name;
        entity.TaxNumber = request.Dto.TaxNumber;
        entity.Email = request.Dto.Email;
        entity.CurrencyType = request.Dto.CurrencyType;
        entity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return new CurrencyAccountDto(
            entity.Id, entity.CompanyId, entity.Code, entity.Name,
            entity.TaxNumber, entity.Email, entity.CurrencyType,
            entity.IsActive, entity.CreatedAt);
    }
}
