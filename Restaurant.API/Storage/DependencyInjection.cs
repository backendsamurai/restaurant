using Amazon.S3;
using Microsoft.Extensions.Options;

namespace Restaurant.API.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddStorageConfiguration(this IServiceCollection services) =>
        services.ConfigureOptions<StorageOptionsSetup>();

    public static IServiceCollection AddS3Storage(this IServiceCollection services) =>
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var storageOptions = sp.GetRequiredService<IOptions<StorageOptions>>()
                ?? throw new InvalidOperationException("Cannot get storage options from configuration");

            var s3Config = new AmazonS3Config
            {
                AuthenticationRegion = storageOptions.Value.Region,
                ServiceURL = storageOptions.Value.ServiceURL,
                ForcePathStyle = true
            };

            return new AmazonS3Client(storageOptions.Value.AccessKey, storageOptions.Value.SecretKey, s3Config);
        });

    public static IServiceCollection AddStorageServices(this IServiceCollection services) =>
        services.AddScoped<IStorageService, StorageService>();
}