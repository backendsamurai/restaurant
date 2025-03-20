using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Restaurant.Shared.Configurations;

public sealed class ManagerOptions
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public sealed class ManagerOptionsSetup(IConfiguration configuration) : IConfigureOptions<ManagerOptions>
{
    public const string SectionName = "Manager";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(ManagerOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
