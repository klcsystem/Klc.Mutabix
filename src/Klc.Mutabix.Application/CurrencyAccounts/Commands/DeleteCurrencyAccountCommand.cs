using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.CurrencyAccounts.Commands;

public record DeleteCurrencyAccountCommand(int Id) : IRequest<bool>;

public class DeleteCurrencyAccountCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteCurrencyAccountCommand, bool>
{
    public async Task<bool> Handle(DeleteCurrencyAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.CurrencyAccounts
            .FirstOrDefaultAsync(ca => ca.Id == request.Id, cancellationToken);

        if (entity is null) return false;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
