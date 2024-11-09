using System.Text.Json;
using System.Text.Json.Serialization;
using Restaurant.API.Attributes;
using Restaurant.API.Caching;
using Restaurant.API.Data;
using Restaurant.API.Mail;
using Restaurant.API.Mapping;
using Restaurant.API.Messaging;
using Restaurant.API.Repositories;
using Restaurant.API.Security;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Services;
using Restaurant.API.Storage;
using Restaurant.API.Types;
using Restaurant.API.Validators;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting application !");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var pgConnString = builder.Configuration.GetConnectionString("PostgreSQL");
    var redisConnString = builder.Configuration.GetConnectionString("RedisCache");
    var jwtOptions = builder.Configuration.GetRequiredSection(JwtOptionsSetup.SectionName).Get<JwtOptions>();

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

    // Logging with Serilog
    builder.Logging.ClearProviders();

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
    );

    // Swagger
    builder.Services.AddSwaggerGen();

    // Core
    builder.Services.AddRepositories()
        .AddInternalServices()
        .AddValidators()
        .AddMappings();

    // Security
    builder.Services.AddSecurityConfigurations()
        .AddSecurityServices()
        .AddSecurityAuthentication(jwtOptions);

    // Database
    builder.Services.AddDatabaseContext(pgConnString);

    // Redis Cache
    builder.Services.AddRedisCaching(redisConnString)
        .AddRedisIndexes()
        .AddRedisModels();

    // Mail
    builder.Services.AddMailConfiguration()
        .AddMailServices()
        .AddMail();

    // Messaging
    builder.Services.AddMessaging();

    // Custom Types
    builder.Services.AddCustomTypes();

    // Custom Attributes
    builder.Services.AddCustomAttributes();

    // AWS S3 storage with Minio Server
    builder.Services.AddStorageConfiguration()
        .AddS3Storage()
        .AddStorageServices();


    await using var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSerilogRequestLogging(x => x.IncludeQueryInRequestPath = true);

    if (builder.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    await app.RunAsync();
    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}