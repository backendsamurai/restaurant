using System.Security.Claims;
using Restaurant.Shared.Common;

namespace Restaurant.Services.Contracts;

public interface IJwtService
{
    public Result<string> GenerateToken(List<Claim> claims);
}
