using System.Text.RegularExpressions;

namespace Restaurant.API.Validators.Helpers;

public static class EmailValidatorHelper
{
    public static bool IsEmailValid(string? value)
    {
        if (value is not null && !string.IsNullOrWhiteSpace(value))
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)" +
                        @"*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+" +
                        @"[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.Match(value, pattern).Success;
        }

        return false;
    }
}
