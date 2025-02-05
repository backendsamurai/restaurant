using Restaurant.API.Security.Configurations;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers.Helpers;

public static class DetectAudienceHeaderHelper
{
    public static Result<string> Detect(IHeaderDictionary headers, JwtOptions jwtOptions)
    {
        string? audience = headers.FirstOrDefault(h => h.Key == "Audience").Value;

        if (string.IsNullOrEmpty(audience) || !jwtOptions.Audiences.Contains(audience))
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Unauthorized)
                .WithSeverity(ErrorSeverity.Warning)
                .WithType("INVALID_AUDIENCE")
                .WithTitle("Unauthorized")
                .WithMessage("Check all provided data and try again")
            );

        return Result.Success(audience);
    }
}
