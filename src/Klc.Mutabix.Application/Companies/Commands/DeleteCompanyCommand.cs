using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Companies.Commands;

public record DeleteCompanyCommand(int Id) : IRequest<bool>;

public class DeleteCompanyCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteCompanyCommand, bool>
{
    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity is null) return false;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
