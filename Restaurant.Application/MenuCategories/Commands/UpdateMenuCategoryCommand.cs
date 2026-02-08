using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuCategory;

namespace Restaurant.Application.MenuCategories.Commands;

public sealed record UpdateMenuCategoryCommand(Guid MenuCategoryId, UpdateMenuCategoryDTO Dto);

public sealed class UpdateMenuCategoryCommandHandler
{
    private readonly IMenuCategoriesService _menuCategoriesService;

    public UpdateMenuCategoryCommandHandler(IMenuCategoriesService menuCategoriesService)
    {
        _menuCategoriesService = menuCategoriesService;
    }

    public async Task<Result<MenuCategory>> HandleAsync(UpdateMenuCategoryCommand cmd, CancellationToken cancellationToken) =>
        await _menuCategoriesService.UpdateMenuCategory(cmd.MenuCategoryId, cmd.Dto, cancellationToken);
}
