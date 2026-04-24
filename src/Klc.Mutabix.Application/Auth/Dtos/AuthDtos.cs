namespace Klc.Mutabix.Application.Auth.Dtos;

public record LoginDto(string Email, string Password);

public record RegisterDto(string Name, string Email, string Password, int? CompanyId);

public record TokenDto(string Token, DateTime Expiration);

public record AuthResponseDto(
    string Token,
    string? RefreshToken,
    DateTime Expiration,
    int UserId,
    string Name,
    string Email,
    List<string> Roles);

public record RefreshTokenDto(string RefreshToken);

public record ForgotPasswordDto(string Email);

public record ResetPasswordDto(string Token, string NewPassword);

public record ChangePasswordDto(string CurrentPassword, string NewPassword);
