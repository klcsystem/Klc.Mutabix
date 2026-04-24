namespace Klc.Mutabix.Application.Users.Dtos;

public record UserDto(
    int Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    List<string> Roles,
    List<int> CompanyIds);

public record CreateUserDto(
    string Name,
    string Email,
    string Password,
    List<int>? RoleIds,
    int? CompanyId);

public record UpdateUserDto(
    string Name,
    string Email,
    List<int>? RoleIds);
