namespace Restaurant.API.Configurations;

public sealed class JwtOptions
{
    public required string Issuer { get; init; }
    public required string[] Audiences { get; init; }
    public required string SecurityKey { get; init; }
}
