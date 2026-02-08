namespace Restaurant.Services.DTOs.MenuItem;

public record CreateMenuItemDTO
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required decimal Price { get; init; }

    public required Guid CategoryId { get; init; }
}
