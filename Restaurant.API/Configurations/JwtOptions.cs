namespace Restaurant.API.Configurations;

public record JwtOptions(string Issuer, string[] Audiences, string SecurityKey);
