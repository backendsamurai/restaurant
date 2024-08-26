using Humanizer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.ValueConverters;

public sealed class UserRoleValueConverter : ValueConverter<UserRole, string>
{
    public UserRoleValueConverter() : base(
        (role) => role.ToString().Humanize(LetterCasing.LowerCase),
        (value) => (UserRole)Enum.Parse(typeof(UserRole), value.Humanize(LetterCasing.Title)))
    { }
}
