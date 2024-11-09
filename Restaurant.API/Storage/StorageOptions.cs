using Microsoft.Extensions.Options;

namespace Restaurant.API.Storage;

public sealed class StorageOptions
{
    public required string Region { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string ServiceURL { get; set; }
}

public sealed class StorageOptionsSetup(IConfiguration configuration) : IConfigureOptions<StorageOptions>
{
    public const string SectionName = "Storage";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(StorageOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}