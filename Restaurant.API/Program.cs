using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Restaurant.API.Attributes;
using Restaurant.API.Mapping;
using Restaurant.API.Security;
using Restaurant.API.Validators;
using Restaurant.Persistence;
using Restaurant.Services;
using Restaurant.Shared;
using Restaurant.Shared.Configurations;
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
    var adminPassword = builder.Configuration.GetValue<string>("Admin:Password");
    var corsOrigins = builder.Configuration.GetValue<string[]>("Cors:Origins") ?? [];

    builder.Services
        .AddControllers(c => c.Filters.Add<TransformResultIntoResponse>())
        .AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                c.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                c.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
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

    builder.Services.AddShared();

    // Core
    builder.Services
        .AddInternalServices()
        .AddValidators()
        .AddMappings();

    // Security
    builder.Services
        .AddSecurityServices()
        .AddAdmin(adminPassword)
        .AddSecurityAuthentication(jwtOptions);

    // CORS
    builder.Services.AddCors(x =>
    {
        x.AddDefaultPolicy(p => p.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(corsOrigins));
    });

    // Database
    builder.Services.AddDbContext<RestaurantDbContext>(options =>
        options
            .UseNpgsql(pgConnString)
            .LogTo(msg => Log.Information(msg), LogLevel.Information, DbContextLoggerOptions.UtcTime | DbContextLoggerOptions.SingleLine)
            .UseSnakeCaseNamingConvention());

    await using var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseCors();

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