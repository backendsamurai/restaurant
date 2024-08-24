using Microsoft.Extensions.Options;

namespace Restaurant.API.Security.Configurations;

public sealed class JwtOptions
{
    public required string Issuer { get; init; }
    public required string[] Audiences { get; init; }
    public required string SecurityKey { get; init; }
    public int ExpireInMinutes { get; set; } = 60;
}

public sealed class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    public const string SectionName = "JWT";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}