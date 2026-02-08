using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuItems.Queries;

public sealed record GetMenuItemByIdQuery(Guid MenuItemId);

public sealed class GetMenuItemByIdQueryHandler
{
    private readonly IMenuItemsService _menuItemsService;

    public GetMenuItemByIdQueryHandler(IMenuItemsService menuItemsService)
    {
        _menuItemsService = menuItemsService;
    }

    public async Task<Result<MenuItem>> HandleAsync(GetMenuItemByIdQuery query, CancellationToken cancellationToken) =>
        await _menuItemsService.GetMenuItemAsync(query.MenuItemId, cancellationToken);
}
