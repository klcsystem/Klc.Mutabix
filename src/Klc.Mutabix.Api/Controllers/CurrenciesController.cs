using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Currencies.Dtos;
using Klc.Mutabix.Application.Currencies.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrenciesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CurrencyDto>>>> GetAll()
    {
        var result = await mediator.Send(new GetCurrenciesQuery());
        return Ok(ApiResponse<List<CurrencyDto>>.Ok(result));
    }
}
