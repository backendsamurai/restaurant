using System.Security.Claims;
using Ardalis.Result;

namespace Restaurant.API.Security.Services.Contracts;

public interface IJwtService
{
    public Result<string> GenerateToken(string audience, List<Claim> claims);
}
