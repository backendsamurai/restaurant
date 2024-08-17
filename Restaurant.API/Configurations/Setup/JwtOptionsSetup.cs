using Microsoft.Extensions.Options;

namespace Restaurant.API.Configurations.Setup;

public sealed class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JWT";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}