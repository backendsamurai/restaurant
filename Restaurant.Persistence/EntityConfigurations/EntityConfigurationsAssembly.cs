using System.Reflection;

namespace Restaurant.Persistence.EntityConfigurations;

public static class EntityConfigurationsAssembly
{
    public static Assembly GetAssembly() =>
        typeof(EntityConfigurationsAssembly).Assembly;
}
