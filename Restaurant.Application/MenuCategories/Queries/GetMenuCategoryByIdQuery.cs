using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuCategories.Queries;

public sealed record GetMenuCategoryByIdQuery(Guid MenuCategoryId);

public sealed class GetMenuCategoryByIdQueryHandler
{
    private readonly IMenuCategoriesService _menuCategoriesService;

    public GetMenuCategoryByIdQueryHandler(IMenuCategoriesService menuCategoriesService)
    {
        _menuCategoriesService = menuCategoriesService;
    }

    public async Task<Result<MenuCategory>> HandleAsync(GetMenuCategoryByIdQuery query, CancellationToken cancellationToken) =>
        await _menuCategoriesService.GetMenuCategoryAsync(query.MenuCategoryId, cancellationToken);
}
