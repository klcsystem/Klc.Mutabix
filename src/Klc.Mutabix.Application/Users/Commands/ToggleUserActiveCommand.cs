using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Users.Commands;

public record ToggleUserActiveCommand(int UserId) : IRequest<bool>;

public class ToggleUserActiveCommandHandler(IApplicationDbContext context)
    : IRequestHandler<ToggleUserActiveCommand, bool>
{
    public async Task<bool> Handle(ToggleUserActiveCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null) return false;

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
