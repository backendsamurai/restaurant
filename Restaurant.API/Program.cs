using System.Text.Json;
using System.Text.Json.Serialization;
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
using Restaurant.API.Types;
using Restaurant.API.Validators;

var builder = WebApplication.CreateBuilder(args);

var pgConnString = builder.Configuration.GetConnectionString("PostgreSQL");
var redisConnString = builder.Configuration.GetConnectionString("RedisCache");
var jwtOptions = builder.Configuration.GetRequiredSection(JwtOptionsSetup.SectionName).Get<JwtOptions>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    var enumConverter = new JsonStringEnumConverter();

    options.JsonSerializerOptions.Converters.Add(enumConverter);
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
