using System.Text;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.Implementations;
using Restaurant.Shared.Configurations;
using Restaurant.Shared.Models;

namespace Restaurant.API.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddAdmin(this IServiceCollection services, string? password)
    {
        return password == null ? throw new ArgumentNullException(nameof(password)) : services.AddSingleton<Admin>((_) => new(password));
    }

    public static IServiceCollection AddSecurityServices(this IServiceCollection services) =>
        services
            .AddSingleton<IPasswordHasherService, PasswordHasherService>()
            .AddSingleton<IOtpGeneratorService, OtpGeneratorService>()
            .AddScoped<IJwtService, JwtService>();

    public static IServiceCollection AddSecurityAuthentication(this IServiceCollection services, JwtOptions? jwtOptions)
    {
        if (jwtOptions is not null)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudiences = jwtOptions.Audiences,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
                };
            });

            services.AddAuthorizationBuilder()
                .AddPolicy(
                    AuthorizationPolicies.RequireCustomer,
                    p => p.RequireClaim(ClaimTypes.UserRole, UserRole.Customer.ToString().Humanize(LetterCasing.LowerCase))
                )
                .AddPolicy(
                    AuthorizationPolicies.RequireAdmin,
                    p => p.RequireClaim(ClaimTypes.UserRole, UserRole.Admin.ToString().Humanize(LetterCasing.LowerCase))
                );
        }

        return services;
    }
}
