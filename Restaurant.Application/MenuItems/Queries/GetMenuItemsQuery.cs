using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuItems.Queries;

public sealed record GetMenuItemsQuery(string? Name);

public sealed class GetMenuItemsQueryHandler
{
    private readonly IMenuItemsService _menuItemsService;

    public GetMenuItemsQueryHandler(IMenuItemsService menuItemsService)
    {
        _menuItemsService = menuItemsService;
    }

    public async Task<Result<List<MenuItem>>> HandleAsync(GetMenuItemsQuery query, CancellationToken cancellationToken) =>
        await _menuItemsService.GetMenuItemsAsync(query.Name, cancellationToken);
}
