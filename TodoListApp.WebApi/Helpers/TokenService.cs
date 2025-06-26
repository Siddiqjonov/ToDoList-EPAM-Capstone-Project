#pragma warning disable SA1200 // Using directives should be placed correctly
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable SA1413 // Use trailing comma in multi-line initializers
#pragma warning disable format

namespace TodoListApp.WebApi.Helpers;

public class TokenService : ITokenService
{
    private readonly IConfiguration config;

    public TokenService(IConfiguration configuration)
    {
        this.config = configuration.GetSection("Jwt");
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public string GenerateTokent(AppUserGetDto user)
    {
        var identityClaims = new Claim[]
        {
            new ("Id", user.UserId.ToString()),
            new ("PhoneNumber", user.PhoneNumber.ToString()),
            new ("UserName", user.UserName.ToString()),
            new (ClaimTypes.Email, user.Email.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["SecurityKey"] !));

        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresHours = int.Parse(this.config["LifeTime"] !);

        var token = new JwtSecurityToken(
            issuer: this.config["Issuer"],
            audience: this.config["Audience"],
            claims: identityClaims,
            expires: TimeHelper.GetDateTime().AddHours(expiresHours),
            signingCredentials: keyCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = this.config["Issuer"],
            ValidateAudience = true,
            ValidAudience = this.config["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["SecurityKey"] !))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }
}
