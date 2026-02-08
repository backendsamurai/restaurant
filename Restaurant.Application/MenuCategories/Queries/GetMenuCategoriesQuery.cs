using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuCategories.Queries;

public sealed record GetMenuCategoriesQuery(string? Name);

public sealed class GetMenuCategoriesQueryHandler
{
    private readonly IMenuCategoriesService _menuCategoriesService;

    public GetMenuCategoriesQueryHandler(IMenuCategoriesService menuCategoriesService)
    {
        _menuCategoriesService = menuCategoriesService;
    }

    public async Task<Result<List<MenuCategory>>> HandleAsync(GetMenuCategoriesQuery query, CancellationToken cancellationToken) =>
        await _menuCategoriesService.GetMenuCategoriesAsync(query.Name, cancellationToken);
}
