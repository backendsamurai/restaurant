using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.ValueConverters;

public sealed class UserRoleValueConverter : ValueConverter<UserRole, string>
{
    public UserRoleValueConverter() : base(
        (role) => role.ToString(),
        (value) => (UserRole)Enum.Parse(typeof(UserRole), value))
    { }
}
