using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Reconciliations.Commands;
using Klc.Mutabix.Application.Reconciliations.Dtos;
using Klc.Mutabix.Application.Reconciliations.Queries;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReconciliationsController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    private string BaseUrl => configuration["App:BaseUrl"] ?? "https://mutabix.klcsystem.com";
    private string? DefaultCc => configuration["Mail:DefaultCc"];

    // Account Reconciliations
    [HttpGet("account/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<List<AccountReconciliationDto>>>> GetAccountReconciliations(int companyId)
    {
        var result = await mediator.Send(new GetAccountReconciliationsQuery(companyId));
        return Ok(ApiResponse<List<AccountReconciliationDto>>.Ok(result));
    }

    [HttpPost("account")]
    public async Task<ActionResult<ApiResponse<AccountReconciliationDto>>> CreateAccountReconciliation(
        [FromBody] CreateAccountReconciliationDto dto)
    {
        var result = await mediator.Send(new CreateAccountReconciliationCommand(dto));
        return Ok(ApiResponse<AccountReconciliationDto>.Ok(result));
    }

    [HttpDelete("account/{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteAccountReconciliation(int id)
    {
        var result = await mediator.Send(new DeleteReconciliationCommand(id, ReconciliationType.AccountReconciliation));
        if (!result) return NotFound(ApiResponse.Fail("Mutabakat bulunamadi"));
        return Ok(ApiResponse.Ok("Mutabakat silindi"));
    }

    [HttpPost("account/{id:int}/send")]
    public async Task<ActionResult<ApiResponse>> SendAccountReconciliationEmail(int id)
    {
        var result = await mediator.Send(new SendReconciliationEmailCommand(id, ReconciliationType.AccountReconciliation, BaseUrl, DefaultCc));
        if (!result) return BadRequest(ApiResponse.Fail("Email gonderilemedi"));
        return Ok(ApiResponse.Ok("Email gonderildi"));
    }

    // BaBs Reconciliations
    [HttpGet("babs/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<List<BaBsReconciliationDto>>>> GetBaBsReconciliations(int companyId)
    {
        var result = await mediator.Send(new GetBaBsReconciliationsQuery(companyId));
        return Ok(ApiResponse<List<BaBsReconciliationDto>>.Ok(result));
    }

    [HttpPost("babs")]
    public async Task<ActionResult<ApiResponse<BaBsReconciliationDto>>> CreateBaBsReconciliation(
        [FromBody] CreateBaBsReconciliationDto dto)
    {
        var result = await mediator.Send(new CreateBaBsReconciliationCommand(dto));
        return Ok(ApiResponse<BaBsReconciliationDto>.Ok(result));
    }

    [HttpDelete("babs/{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteBaBsReconciliation(int id)
    {
        var result = await mediator.Send(new DeleteReconciliationCommand(id, ReconciliationType.BaBsReconciliation));
        if (!result) return NotFound(ApiResponse.Fail("Mutabakat bulunamadi"));
        return Ok(ApiResponse.Ok("Mutabakat silindi"));
    }

    [HttpPost("babs/{id:int}/send")]
    public async Task<ActionResult<ApiResponse>> SendBaBsReconciliationEmail(int id)
    {
        var result = await mediator.Send(new SendReconciliationEmailCommand(id, ReconciliationType.BaBsReconciliation, BaseUrl, DefaultCc));
        if (!result) return BadRequest(ApiResponse.Fail("Email gonderilemedi"));
        return Ok(ApiResponse.Ok("Email gonderildi"));
    }

    // Stats
    [HttpGet("stats/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<ReconciliationStatsDto>>> GetStats(int companyId)
    {
        var result = await mediator.Send(new GetReconciliationStatsQuery(companyId));
        return Ok(ApiResponse<ReconciliationStatsDto>.Ok(result));
    }

    // Public counterparty response endpoint (no auth required)
    [AllowAnonymous]
    [HttpPost("respond/{guid}")]
    public async Task<ActionResult<ApiResponse>> Respond(string guid, [FromBody] RespondToReconciliationDto dto)
    {
        var result = await mediator.Send(new RespondToReconciliationCommand(guid, dto));
        if (!result) return NotFound(ApiResponse.Fail("Mutabakat bulunamadi veya suresi dolmus"));
        var message = dto.IsApproved ? "Mutabakat onaylandi" : "Mutabakat reddedildi";
        return Ok(ApiResponse.Ok(message));
    }
}
