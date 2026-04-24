namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    public ICollection<UserCompany> UserCompanies { get; set; } = [];
    public ICollection<UserOperationClaim> UserOperationClaims { get; set; } = [];
}
