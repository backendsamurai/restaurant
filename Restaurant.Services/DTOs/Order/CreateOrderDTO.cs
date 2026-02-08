namespace Restaurant.Services.DTOs.Order;

public record CreateOrderDTO
{
    public required Guid ConsumerId { get; init; }

    public required Dictionary<Guid, int> LineItems { get; init; }
}

