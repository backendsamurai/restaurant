using Microsoft.Extensions.Options;

namespace Restaurant.API.Configurations.Setup;

public sealed class ManagerOptionsSetup(IConfiguration configuration) : IConfigureOptions<ManagerOptions>
{
    private const string SectionName = "Manager";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(ManagerOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
