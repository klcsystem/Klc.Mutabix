using Klc.Mutabix.Application.Common.Models;
using Klc.Mutabix.Application.Users.Commands;
using Klc.Mutabix.Application.Users.Dtos;
using Klc.Mutabix.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAll()
    {
        var result = await mediator.Send(new GetAllUsersQuery());
        return Ok(ApiResponse<List<UserDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));
        if (result is null)
            return NotFound(ApiResponse<UserDto>.Fail("Kullanici bulunamadi"));
        return Ok(ApiResponse<UserDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] CreateUserDto dto)
    {
        var result = await mediator.Send(new CreateUserCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<UserDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await mediator.Send(new UpdateUserCommand(id, dto));
        if (result is null)
            return NotFound(ApiResponse<UserDto>.Fail("Kullanici bulunamadi"));
        return Ok(ApiResponse<UserDto>.Ok(result));
    }

    [HttpPatch("{id:int}/toggle-active")]
    public async Task<ActionResult<ApiResponse>> ToggleActive(int id)
    {
        var result = await mediator.Send(new ToggleUserActiveCommand(id));
        if (!result)
            return NotFound(ApiResponse.Fail("Kullanici bulunamadi"));
        return Ok(ApiResponse.Ok("Kullanici durumu degistirildi"));
    }
}
