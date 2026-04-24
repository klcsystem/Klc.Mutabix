using System.Security.Claims;
using Klc.Mutabix.Application.Auth.Commands;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klc.Mutabix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { Message = "Mutabix API is running", Timestamp = DateTime.UtcNow });

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var result = await mediator.Send(new LoginCommand(dto));
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto dto)
    {
        var result = await mediator.Send(new RegisterCommand(dto));
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var result = await mediator.Send(new RefreshTokenCommand(dto));
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        await mediator.Send(new ForgotPasswordCommand(dto));
        return Ok(ApiResponse.Ok("Sifre sifirlama linki e-posta adresinize gonderildi"));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        await mediator.Send(new ResetPasswordCommand(dto));
        return Ok(ApiResponse.Ok("Sifreniz basariyla degistirildi"));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await mediator.Send(new ChangePasswordCommand(userId, dto));
        return Ok(ApiResponse.Ok("Sifreniz basariyla degistirildi"));
    }
}
