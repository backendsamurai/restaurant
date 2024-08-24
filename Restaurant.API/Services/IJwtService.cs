using System.Security.Claims;
using Ardalis.Result;

namespace Restaurant.API.Services;

public interface IJwtService
{
    public Result<string> GenerateToken(string audience, List<Claim> claims);
}
