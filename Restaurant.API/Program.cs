using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

app.MapControllers();

app.Run();
