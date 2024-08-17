using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Configurations;
using Restaurant.API.Configurations.Setup;
using Restaurant.API.Data;
using Restaurant.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<ManagerOptionsSetup>();

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.Get<JwtOptions>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions?.Issuer,
        ValidAudiences = jwtOptions?.Audiences,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecurityKey ?? ""))
    };
});

builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
