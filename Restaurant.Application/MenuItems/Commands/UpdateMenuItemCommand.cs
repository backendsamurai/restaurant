using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuItem;

namespace Restaurant.Application.MenuItems.Commands;

public sealed record UpdateMenuItemCommand(Guid MenuItemId, UpdateMenuItemDTO Dto);

public sealed class UpdateMenuItemCommandHandler
{
    private readonly IMenuItemsService _menuItemsService;

    public UpdateMenuItemCommandHandler(IMenuItemsService menuItemsService)
    {
        _menuItemsService = menuItemsService;
    }

    public async Task<Result<MenuItem>> HandleAsync(UpdateMenuItemCommand cmd, CancellationToken cancellationToken) =>
        await _menuItemsService.UpdateMenuItemAsync(cmd.MenuItemId, cmd.Dto, cancellationToken);
}
