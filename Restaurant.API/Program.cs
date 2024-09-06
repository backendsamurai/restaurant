using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Caching;
using Restaurant.API.Data;
using Restaurant.API.Mapping;
using Restaurant.API.Repositories;
using Restaurant.API.Security;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Services;
using Restaurant.API.Validators;

var builder = WebApplication.CreateBuilder(args);

var pgConnString = builder.Configuration.GetConnectionString("PostgreSQL");
var redisConnString = builder.Configuration.GetConnectionString("RedisCache");
var jwtOptions = builder.Configuration.GetRequiredSection(JwtOptionsSetup.SectionName).Get<JwtOptions>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services
    .AddRepositories()
    .AddInternalServices()
    .AddValidators()
    .AddMappings()
    .AddSecurityConfigurations()
    .AddSecurityServices()
    .AddSecurityAuthentication(jwtOptions)
    .AddDatabaseContext(pgConnString)
    .AddRedisCaching(redisConnString)
    .AddRedisIndexes()
    .AddRedisModels();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
