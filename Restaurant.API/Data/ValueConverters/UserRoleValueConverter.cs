using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.ValueConverters;

public sealed class UserRoleValueConverter : ValueConverter<UserRole, string>
{
    public UserRoleValueConverter() : base(
        (role) => role.ToString().ToLower(),
        (value) => (UserRole)Enum.Parse(typeof(UserRole), value.ToUpper()))
    { }
}
