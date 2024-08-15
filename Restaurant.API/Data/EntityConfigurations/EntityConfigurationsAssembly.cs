using System.Reflection;

namespace Restaurant.API.Data.EntityConfigurations;

public static class EntityConfigurationsAssembly
{
    public static Assembly GetAssembly() =>
        typeof(EntityConfigurationsAssembly).Assembly;
}
