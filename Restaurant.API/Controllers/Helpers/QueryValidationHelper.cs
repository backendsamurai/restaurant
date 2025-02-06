using System.Text.RegularExpressions;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers.Helpers;

public static partial class QueryValidationHelper
{
    [GeneratedRegex(@"^(?![_,-,0-9])[a-zA-Z0-9]*")]
    private static partial Regex QueryValidationRegex();

    public static Result Validate(string value)
    {
        if (value.Length < 2)
            return DetailedError.InvalidQuery("The query value must contain two or more characters");

        if (!QueryValidationRegex().IsMatch(value))
            return DetailedError.InvalidQuery("The query value must start with letters only");

        return Result.Success();
    }
}
