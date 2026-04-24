using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.MailParameters.Commands;
using Klc.Mutabix.Application.MailParameters.Dtos;
using Klc.Mutabix.Application.MailParameters.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MailParametersController(IMediator mediator) : ControllerBase
{
    [HttpGet("company/{companyId:int}")]
    public async Task<ActionResult<ApiResponse<MailParameterDto>>> GetByCompany(int companyId)
    {
        var result = await mediator.Send(new GetMailParameterByCompanyQuery(companyId));
        if (result is null)
            return NotFound(ApiResponse<MailParameterDto>.Fail("Mail parametreleri bulunamadi"));
        return Ok(ApiResponse<MailParameterDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MailParameterDto>>> Create([FromBody] CreateMailParameterDto dto)
    {
        var result = await mediator.Send(new CreateMailParameterCommand(dto));
        return CreatedAtAction(nameof(GetByCompany), new { companyId = result.CompanyId },
            ApiResponse<MailParameterDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<MailParameterDto>>> Update(int id, [FromBody] UpdateMailParameterDto dto)
    {
        var result = await mediator.Send(new UpdateMailParameterCommand(id, dto));
        if (result is null)
            return NotFound(ApiResponse<MailParameterDto>.Fail("Mail parametreleri bulunamadi"));
        return Ok(ApiResponse<MailParameterDto>.Ok(result));
    }
}
