namespace Restaurant.API.Configurations;

public sealed class ManagerOptions
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}