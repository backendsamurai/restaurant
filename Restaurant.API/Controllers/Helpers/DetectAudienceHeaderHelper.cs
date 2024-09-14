using Restaurant.API.Security.Configurations;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers.Helpers;

public static class DetectAudienceHeaderHelper
{
    public static Result<string> Detect(IHeaderDictionary headers, JwtOptions jwtOptions)
    {
        string? audience = headers.FirstOrDefault(h => h.Key == "Audience").Value;

        if (string.IsNullOrEmpty(audience))
            return Result.Error(
                code: "0015",
                type: "missing_audience_header",
                message: "Missing Audience Header",
                detail: "Audience header not set or value is empty"
            );

        if (!jwtOptions.Audiences.Contains(audience))
            return Result.Error(
                code: "0015",
                type: "incorrect_audience_header",
                message: "Incorrect Audience Header",
                detail: "Incorrect audience value in header"
            );

        return Result.Success(audience);
    }
}
