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
public class ErpConnectionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("company/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<List<ErpConnectionDto>>>> GetByCompany(int companyId)
    {
        var result = await mediator.Send(new GetErpConnectionsQuery(companyId));
        return Ok(ApiResponse<List<ErpConnectionDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ErpConnectionDto>>> Create([FromBody] CreateErpConnectionDto dto)
    {
        var result = await mediator.Send(new CreateErpConnectionCommand(dto));
        return Ok(ApiResponse<ErpConnectionDto>.Ok(result));
    }

    [HttpPost("{id:int}/test")]
    public async Task<ActionResult<ApiResponse>> TestConnection(int id)
    {
        var result = await mediator.Send(new TestErpConnectionCommand(id));
        return Ok(result
            ? ApiResponse.Ok("Baglanti basarili")
            : ApiResponse.Fail("Baglanti basarisiz"));
    }
}
