using System.Security.Claims;
using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Notifications.Commands;
using Klc.Mutabix.Application.Notifications.Dtos;
using Klc.Mutabix.Application.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<NotificationDto>>>> GetMy([FromQuery] bool unreadOnly = false)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetNotificationsQuery(userId, unreadOnly));
        return Ok(ApiResponse<List<NotificationDto>>.Ok(result));
    }

    [HttpPut("{id:int}/read")]
    public async Task<ActionResult<ApiResponse>> MarkRead(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new MarkNotificationReadCommand(id, userId));
        if (!result) return NotFound(ApiResponse.Fail("Bildirim bulunamadi"));
        return Ok(ApiResponse.Ok("Bildirim okundu olarak isaretlendi"));
    }
}
