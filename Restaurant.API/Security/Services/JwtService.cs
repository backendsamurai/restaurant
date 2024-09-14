using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Types;

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
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
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
            return Result.Error(
                code: "0010",
                type: "token_generation_error",
                message: "Cannot generate authentication token",
                detail: "Please check provided credentials and try again later"
            );
        }
    }
}
