using System.Text.RegularExpressions;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers.Helpers;

public static class QueryValidationHelper
{
    public static Result Validate(string value)
    {
        if (value.Length < 2)
            return DetailedError.InvalidQuery("The query value must contain two or more characters");

        if (!Regex.Match(value, @"^(?![_,-,0-9])[a-zA-Z0-9]*").Success)
            return DetailedError.InvalidQuery("The query value must start with letters only");

        return Result.Success();
    }
}
