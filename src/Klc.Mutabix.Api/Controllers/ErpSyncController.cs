using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Erp.Commands;
using Klc.Mutabix.Application.Erp.Dtos;
using Klc.Mutabix.Application.Erp.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ErpSyncController(IMediator mediator) : ControllerBase
{
    [HttpPost("{connectionId:int}")]
    public async Task<ActionResult<ApiResponse<ErpSyncLogDto>>> Sync(int connectionId)
    {
        var result = await mediator.Send(new SyncErpDataCommand(connectionId));
        return Ok(ApiResponse<ErpSyncLogDto>.Ok(result));
    }

    [HttpGet("{connectionId:int}/logs")]
    public async Task<ActionResult<ApiResponse<List<ErpSyncLogDto>>>> GetLogs(int connectionId, [FromQuery] int take = 20)
    {
        var result = await mediator.Send(new GetErpSyncLogsQuery(connectionId, take));
        return Ok(ApiResponse<List<ErpSyncLogDto>>.Ok(result));
    }
}
