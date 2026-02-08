namespace Restaurant.Services.DTOs.MenuItem;

public record UpdateMenuItemDTO
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public decimal? Price { get; init; }

    public Guid? CategoryId { get; init; }
}
