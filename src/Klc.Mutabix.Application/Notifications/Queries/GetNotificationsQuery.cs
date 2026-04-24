using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Notifications.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Notifications.Queries;

public record GetNotificationsQuery(int UserId, bool UnreadOnly = false) : IRequest<List<NotificationDto>>;

public class GetNotificationsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
    public async Task<List<NotificationDto>> Handle(
        GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Notifications
            .Where(n => n.UserId == request.UserId);

        if (request.UnreadOnly)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto(
                n.Id, n.Title, n.Message, n.Link, n.IsRead, n.ReadAt, n.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
