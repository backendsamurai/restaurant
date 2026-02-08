using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuCategory;

namespace Restaurant.Application.MenuCategories.Commands;

public sealed record CreateMenuCategoryCommand(CreateMenuCategoryDTO Dto);

public sealed class CreateMenuCategoryCommandHandler
{
    private readonly IMenuCategoriesService _menuCategoriesService;

    public CreateMenuCategoryCommandHandler(IMenuCategoriesService menuCategoriesService)
    {
        _menuCategoriesService = menuCategoriesService;
    }

    public async Task<Result<MenuCategory>> HandleAsync(CreateMenuCategoryCommand cmd, CancellationToken cancellationToken) =>
        await _menuCategoriesService.CreateMenuCategory(cmd.Dto, cancellationToken);
}
