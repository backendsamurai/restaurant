namespace Restaurant.Services.DTOs.Consumer;

public record UpdateConsumerDTO
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }
}
