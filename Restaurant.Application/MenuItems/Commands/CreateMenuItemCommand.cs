using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuItem;

namespace Restaurant.Application.MenuItems.Commands;

public sealed record CreateMenuItemCommand(CreateMenuItemDTO Dto);

public sealed class CreateMenuItemCommandHandler
{
    private readonly IMenuItemsService _menuItemsService;

    public CreateMenuItemCommandHandler(IMenuItemsService menuItemsService)
    {
        _menuItemsService = menuItemsService;
    }

    public async Task<Result<MenuItem>> HandleAsync(CreateMenuItemCommand cmd, CancellationToken cancellationToken) =>
        await _menuItemsService.CreateMenuItemAsync(cmd.Dto, cancellationToken);
}
