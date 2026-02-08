using Ardalis.Result;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.MenuItems.Commands;

public sealed record RemoveMenuItemCommand(Guid MenuItemId);

public sealed class RemoveMenuItemCommandHandler
{
    private readonly IMenuItemsService _menuItemsService;

    public RemoveMenuItemCommandHandler(IMenuItemsService menuItemsService)
    {
        _menuItemsService = menuItemsService;
    }

    public async Task<Result> HandleAsync(RemoveMenuItemCommand cmd, CancellationToken cancellationToken) =>
        await _menuItemsService.RemoveMenuItem(cmd.MenuItemId, cancellationToken);
}
