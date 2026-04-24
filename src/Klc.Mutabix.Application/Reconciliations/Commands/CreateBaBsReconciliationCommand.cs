using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record CreateBaBsReconciliationCommand(CreateBaBsReconciliationDto Dto)
    : IRequest<BaBsReconciliationDto>;

public class CreateBaBsReconciliationCommandValidator
    : AbstractValidator<CreateBaBsReconciliationCommand>
{
    public CreateBaBsReconciliationCommandValidator()
    {
        RuleFor(x => x.Dto.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.CurrencyAccountId).GreaterThan(0);
        RuleFor(x => x.Dto.Type).IsInEnum();
        RuleFor(x => x.Dto.Year).InclusiveBetween(2000, 2100);
        RuleFor(x => x.Dto.Month).InclusiveBetween(1, 12);
    }
}

public class CreateBaBsReconciliationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateBaBsReconciliationCommand, BaBsReconciliationDto>
{
    public async Task<BaBsReconciliationDto> Handle(
        CreateBaBsReconciliationCommand request, CancellationToken cancellationToken)
    {
        var entity = new BaBsReconciliation
        {
            CompanyId = request.Dto.CompanyId,
            CurrencyAccountId = request.Dto.CurrencyAccountId,
            Type = request.Dto.Type,
            Year = request.Dto.Year,
            Month = request.Dto.Month,
            Amount = request.Dto.Amount,
            Quantity = request.Dto.Quantity,
            Guid = System.Guid.NewGuid().ToString()
        };

        context.BaBsReconciliations.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        var accountName = await context.CurrencyAccounts
            .Where(ca => ca.Id == entity.CurrencyAccountId)
            .Select(ca => ca.Name)
            .FirstOrDefaultAsync(cancellationToken) ?? "";

        return new BaBsReconciliationDto(
            entity.Id, entity.CompanyId, entity.CurrencyAccountId, accountName,
            entity.Type, entity.Year, entity.Month, entity.Amount, entity.Quantity,
            entity.Status, entity.Guid, entity.IsSent, entity.SentDate, entity.CreatedAt);
    }
}
