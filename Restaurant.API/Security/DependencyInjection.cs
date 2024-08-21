using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Security.Configurations;

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
        }

        return services;
    }
}
