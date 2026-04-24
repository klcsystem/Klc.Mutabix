using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Companies.Commands;
using Klc.Mutabix.Application.Companies.Dtos;
using Klc.Mutabix.Application.Companies.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CompanyDto>>>> GetAll()
    {
        var result = await mediator.Send(new GetCompaniesQuery());
        return Ok(ApiResponse<List<CompanyDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> GetById(int id)
    {
        var result = await mediator.Send(new GetCompanyByIdQuery(id));
        if (result is null)
            return NotFound(ApiResponse<CompanyDto>.Fail("Firma bulunamadi"));
        return Ok(ApiResponse<CompanyDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> Create([FromBody] CreateCompanyDto dto)
    {
        var result = await mediator.Send(new CreateCompanyCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<CompanyDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> Update(int id, [FromBody] UpdateCompanyDto dto)
    {
        var result = await mediator.Send(new UpdateCompanyCommand(id, dto));
        if (result is null)
            return NotFound(ApiResponse<CompanyDto>.Fail("Firma bulunamadi"));
        return Ok(ApiResponse<CompanyDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await mediator.Send(new DeleteCompanyCommand(id));
        if (!result)
            return NotFound(ApiResponse.Fail("Firma bulunamadi"));
        return Ok(ApiResponse.Ok("Firma silindi"));
    }
}
