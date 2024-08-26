using System.Text;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Entities;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Security.Models;
using SystemClaims = System.Security.Claims;

namespace Restaurant.API.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddSecurityConfigurations(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<JwtOptionsSetup>()
            .ConfigureOptions<ManagerOptionsSetup>();
    }

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
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudiences = jwtOptions.Audiences,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
                };
            });

            services.AddAuthorizationBuilder()
                .AddPolicy(
                    AuthorizationPolicies.RequireEmployee,
                    p => p.RequireClaim(ClaimTypes.UserRole, UserRole.Employee.ToString().Humanize(LetterCasing.LowerCase))
                )
                .AddPolicy(
                    AuthorizationPolicies.RequireCustomer,
                    p => p.RequireClaim(ClaimTypes.UserRole, UserRole.Customer.ToString().Humanize(LetterCasing.LowerCase))
                )
                .AddPolicy(AuthorizationPolicies.RequireEmployeeManager, p =>
                {
                    p.RequireClaim(ClaimTypes.UserRole, UserRole.Employee.ToString().Humanize(LetterCasing.LowerCase));
                    p.RequireClaim(ClaimTypes.EmployeeRole, "manager");
                })
                .AddPolicy(AuthorizationPolicies.RequireEmployeeWaiter, p =>
                {
                    p.RequireClaim(ClaimTypes.UserRole, UserRole.Employee.ToString().Humanize(LetterCasing.LowerCase));
                    p.RequireClaim(ClaimTypes.EmployeeRole, "waiter");
                });
        }

        return services;
    }
}
