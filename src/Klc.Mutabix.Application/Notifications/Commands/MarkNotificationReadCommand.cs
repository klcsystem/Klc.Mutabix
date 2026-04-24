using Klc.Mutabix.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Notifications.Commands;

public record MarkNotificationReadCommand(int NotificationId, int UserId) : IRequest<bool>;

public class MarkNotificationReadCommandHandler(IApplicationDbContext context)
    : IRequestHandler<MarkNotificationReadCommand, bool>
{
    public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == request.UserId, cancellationToken);

        if (notification is null) return false;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
