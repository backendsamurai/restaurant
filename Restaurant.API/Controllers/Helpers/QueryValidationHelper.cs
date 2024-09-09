using System.Text.RegularExpressions;
using Ardalis.Result;

namespace Restaurant.API.Controllers.Helpers;

public static class QueryValidationHelper
{
    public static Result Validate(string value)
    {
        if (value.Length < 2)
            return Result.Invalid(new ValidationError("the query value must contain two or more characters"));

        if (!Regex.Match(value, @"^(?![_,-,0-9])[a-zA-Z0-9]*").Success)
            return Result.Invalid(new ValidationError("the query value must start with letters only"));

        return Result.Success();
    }
}
