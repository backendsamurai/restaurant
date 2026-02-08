namespace Restaurant.Services.DTOs.MenuCategory;

public record CreateMenuCategoryDTO
{
    public required string Name { get; init; }
}
