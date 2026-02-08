using Ardalis.Result;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuCategories.Commands;

public sealed record RemoveMenuCategoryCommand(Guid MenuCategoryId);

public sealed class RemoveMenuCategoryCommandHandler
{
    private readonly IMenuCategoriesService _menuCategoriesService;

    public RemoveMenuCategoryCommandHandler(IMenuCategoriesService menuCategoriesService)
    {
        _menuCategoriesService = menuCategoriesService;
    }

    public async Task<Result> HandleAsync(RemoveMenuCategoryCommand cmd, CancellationToken cancellationToken) =>
       await _menuCategoriesService.RemoveMenuCategory(cmd.MenuCategoryId, cancellationToken);
}
