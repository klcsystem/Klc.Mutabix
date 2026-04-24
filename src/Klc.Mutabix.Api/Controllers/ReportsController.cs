using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Reports.Dtos;
using Klc.Mutabix.Application.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController(IMediator mediator) : ControllerBase
{
    [HttpGet("summary/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<ReconciliationSummaryDto>>> GetSummary(int companyId)
    {
        var result = await mediator.Send(new GetReconciliationSummaryQuery(companyId));
        return Ok(ApiResponse<ReconciliationSummaryDto>.Ok(result));
    }

    [HttpGet("trend/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<List<MonthlyTrendDto>>>> GetTrend(int companyId, [FromQuery] int months = 12)
    {
        var result = await mediator.Send(new GetMonthlyTrendQuery(companyId, months));
        return Ok(ApiResponse<List<MonthlyTrendDto>>.Ok(result));
    }
}
