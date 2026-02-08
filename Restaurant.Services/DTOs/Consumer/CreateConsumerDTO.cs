namespace Restaurant.Services.DTOs.Consumer;

public record CreateConsumerDTO
{
    public required string Name { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }
}
