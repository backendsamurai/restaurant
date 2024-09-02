using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.Result;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Security.Services.Contracts;

namespace Restaurant.API.Security.Services;

public sealed class JwtService(IOptions<JwtOptions> jwtOptions) : IJwtService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public Result<string> GenerateToken(string audience, List<Claim> claims)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);

        var tokenDescription = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireInMinutes),
            signingCredentials: credentials
        );

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescription);

            return Result.Success(token);
        }
        catch
        {
            return Result.Error("cannot generate jwt token");
        }
    }
}
