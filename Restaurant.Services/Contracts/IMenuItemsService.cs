using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.DTOs.MenuItem;

namespace Restaurant.Services.Contracts;

public interface IMenuItemsService
{
    Task<Result<List<MenuItem>>> GetMenuItemsAsync(string? name = null, CancellationToken cancellationToken = default);

    Task<Result<MenuItem>> GetMenuItemAsync(Guid menuItemId, CancellationToken cancellationToken = default);

    Task<Result<MenuItem>> CreateMenuItemAsync(
        CreateMenuItemDTO createMenuItemDTO,
        CancellationToken cancellationToken = default);

    Task<Result<MenuItem>> UpdateMenuItemAsync(
        Guid menuItemId,
        UpdateMenuItemDTO updateMenuItemDTO,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveMenuItem(Guid menuItemId, CancellationToken cancellationToken = default);
}
