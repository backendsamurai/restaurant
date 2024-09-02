using Ardalis.Result;
using Restaurant.API.Security.Configurations;

namespace Restaurant.API.Controllers.Helpers;

public static class DetectAudienceHeaderHelper
{
    public static Result<string> Detect(IHeaderDictionary headers, JwtOptions jwtOptions)
    {
        string? audience = headers.FirstOrDefault(h => h.Key == "Audience").Value;

        if (string.IsNullOrEmpty(audience))
            return Result.Error("audience header not set or value is empty");

        if (!jwtOptions.Audiences.Contains(audience))
            return Result.Error("incorrect audience value in header");

        return Result.Success(audience);
    }
}
