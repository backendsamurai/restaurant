using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.DTOs.MenuCategory;

namespace Restaurant.Services.Contracts;

public interface IMenuCategoriesService
{
    Task<Result<List<MenuCategory>>> GetMenuCategoriesAsync(string? name = null, CancellationToken cancellationToken = default);

    Task<Result<MenuCategory>> GetMenuCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default);

    Task<Result<MenuCategory>> CreateMenuCategory(
        CreateMenuCategoryDTO createMenuCategoryDTO,
        CancellationToken cancellationToken = default);

    Task<Result<MenuCategory>> UpdateMenuCategory(
        Guid menuCategoryId, UpdateMenuCategoryDTO updateMenuCategoryDTO,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveMenuCategory(Guid menuCategoryId, CancellationToken cancellationToken = default);
}
