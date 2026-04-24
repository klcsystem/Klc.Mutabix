using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Reconciliations.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController(IMediator mediator) : ControllerBase
{
    [HttpPost("auto-match/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<AutoMatchResult>>> AutoMatch(int companyId)
    {
        var result = await mediator.Send(new AutoMatchReconciliationsCommand(companyId));
        return Ok(ApiResponse<AutoMatchResult>.Ok(result,
            $"{result.Matched} mutabakat otomatik eslestirildi, {result.Unmatched} eslestirilmedi"));
    }

    [HttpPost("bulk-reminder/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<int>>> SendBulkReminder(int companyId)
    {
        var result = await mediator.Send(new SendBulkReminderCommand(companyId));
        return Ok(ApiResponse<int>.Ok(result, $"{result} hatirlatma emaili gonderildi"));
    }
}
