using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.Result.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.API;
using Restaurant.API.Mapping;
using Restaurant.Application;
using Restaurant.Persistence;
using Restaurant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(opt => opt.AddDefaultResultConvention())
    .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

builder.Services
    .RegisterOpenApiDefinition("orders", "Restaurant API - Orders")
    .RegisterOpenApiDefinition("consumers", "Restaurant API - Consumers")
    .RegisterOpenApiDefinition("menuItems", "Restaurant API - Menu Items")
    .RegisterOpenApiDefinition("menuCategories", "Restaurant API - Menu Categories")
    .RegisterOpenApiDefinition("administration", "Restaurant API - Administration");


builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// Core
builder.Services
    .AddInternalServices()
    .AddApplicationLayer()
    .AddMappings();

await using var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/orders.json", "Restaurant API v1 (Orders)");
        options.SwaggerEndpoint("/openapi/consumers.json", "Restaurant API v1 (Consumers)");
        options.SwaggerEndpoint("/openapi/menuItems.json", "Restaurant API v1 (Menu Items)");
        options.SwaggerEndpoint("/openapi/menuCategories.json", "Restaurant API v1 (Menu Categories)");
        options.SwaggerEndpoint("/openapi/administration.json", "Restaurant API v1 (Administration)");

        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
        options.DefaultModelsExpandDepth(-1);
    });
}

app.MapControllers();

await app.RunAsync();
