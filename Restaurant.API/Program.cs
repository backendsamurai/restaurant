using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Caching;
using Restaurant.API.Data;
using Restaurant.API.Mail;
using Restaurant.API.Mapping;
using Restaurant.API.Messaging;
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
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
