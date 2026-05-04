using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record CreateAccountReconciliationCommand(CreateAccountReconciliationDto Dto)
    : IRequest<AccountReconciliationDto>;

public class CreateAccountReconciliationCommandValidator
    : AbstractValidator<CreateAccountReconciliationCommand>
{
    public CreateAccountReconciliationCommandValidator()
    {
        RuleFor(x => x.Dto.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.CurrencyAccountId).GreaterThan(0);
        RuleFor(x => x.Dto.StartDate).LessThan(x => x.Dto.EndDate);
        RuleFor(x => x.Dto.CurrencyType).IsInEnum();
    }
}

public class CreateAccountReconciliationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateAccountReconciliationCommand, AccountReconciliationDto>
{
    public async Task<AccountReconciliationDto> Handle(
        CreateAccountReconciliationCommand request, CancellationToken cancellationToken)
    {
        var entity = new AccountReconciliation
        {
            CompanyId = request.Dto.CompanyId,
            CurrencyAccountId = request.Dto.CurrencyAccountId,
            StartDate = request.Dto.StartDate,
            EndDate = request.Dto.EndDate,
            CurrencyType = request.Dto.CurrencyType,
            DebitAmount = request.Dto.DebitAmount,
            CreditAmount = request.Dto.CreditAmount,
            Guid = System.Guid.NewGuid().ToString()
        };

        context.AccountReconciliations.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        var account = await context.CurrencyAccounts
            .Where(ca => ca.Id == entity.CurrencyAccountId)
            .Select(ca => new { ca.Name, ca.Email })
            .FirstOrDefaultAsync(cancellationToken);

        return new AccountReconciliationDto(
            entity.Id, entity.CompanyId, entity.CurrencyAccountId,
            account?.Name ?? "", account?.Email,
            entity.StartDate, entity.EndDate, entity.CurrencyType,
            entity.DebitAmount, entity.CreditAmount,
            entity.Status, entity.Guid, entity.IsSent, entity.SentDate, entity.CreatedAt);
    }
}
