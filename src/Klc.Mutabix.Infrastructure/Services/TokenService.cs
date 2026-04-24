using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Klc.Mutabix.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(User user, List<OperationClaim> operationClaims)
    {
        var tokenOptions = configuration.GetSection("Jwt");
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenOptions["Secret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        claims.AddRange(operationClaims.Select(c =>
            new Claim(ClaimTypes.Role, c.Name)));

        var token = new JwtSecurityToken(
            issuer: tokenOptions["Issuer"],
            audience: tokenOptions["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(tokenOptions["ExpirationMinutes"] ?? "60")),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
