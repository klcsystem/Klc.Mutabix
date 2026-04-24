using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.CurrencyAccounts.Commands;
using Klc.Mutabix.Application.CurrencyAccounts.Dtos;
using Klc.Mutabix.Application.CurrencyAccounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyAccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet("company/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<List<CurrencyAccountDto>>>> GetByCompany(int companyId)
    {
        var result = await mediator.Send(new GetCurrencyAccountsQuery(companyId));
        return Ok(ApiResponse<List<CurrencyAccountDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CurrencyAccountDto>>> GetById(int id)
    {
        var result = await mediator.Send(new GetCurrencyAccountByIdQuery(id));
        if (result is null)
            return NotFound(ApiResponse<CurrencyAccountDto>.Fail("Cari hesap bulunamadi"));
        return Ok(ApiResponse<CurrencyAccountDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CurrencyAccountDto>>> Create([FromBody] CreateCurrencyAccountDto dto)
    {
        var result = await mediator.Send(new CreateCurrencyAccountCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<CurrencyAccountDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CurrencyAccountDto>>> Update(int id, [FromBody] UpdateCurrencyAccountDto dto)
    {
        var result = await mediator.Send(new UpdateCurrencyAccountCommand(id, dto));
        if (result is null)
            return NotFound(ApiResponse<CurrencyAccountDto>.Fail("Cari hesap bulunamadi"));
        return Ok(ApiResponse<CurrencyAccountDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await mediator.Send(new DeleteCurrencyAccountCommand(id));
        if (!result)
            return NotFound(ApiResponse.Fail("Cari hesap bulunamadi"));
        return Ok(ApiResponse.Ok("Cari hesap silindi"));
    }
}
