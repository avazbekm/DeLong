using DeLong.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeLong.Service.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = _config["JwtSettings:SecretKey"]
            ?? throw new ArgumentNullException("JWT SecretKey topilmadi!");
        if (secretKey.Length < 32)
            throw new ArgumentException("JWT SecretKey kamida 32 ta belgidan iborat bo‘lishi kerak!");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"] ?? "DeLongAPI",
            audience: _config["JwtSettings:Audience"] ?? "DeLongClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // 8 soatlik muddat
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}