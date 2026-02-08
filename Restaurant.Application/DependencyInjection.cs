using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Restaurant.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services) =>
            services.AddWolverine(ExtensionDiscovery.ManualOnly, opt =>
            {
                opt.ApplicationAssembly = typeof(DependencyInjection).Assembly;
                opt.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Auto;
                opt.AutoBuildMessageStorageOnStartup = JasperFx.AutoCreate.All;
            });
    }
}
