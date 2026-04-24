namespace Klc.Mutabix.Application.Notifications.Dtos;

public record NotificationDto(
    int Id,
    string Title,
    string Message,
    string? Link,
    bool IsRead,
    DateTime? ReadAt,
    DateTime CreatedAt);
