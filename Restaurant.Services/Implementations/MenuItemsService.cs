using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;
using Restaurant.Persistence;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuItem;

namespace Restaurant.Services.Implementations;

public sealed class MenuItemsService : IMenuItemsService
{
    private readonly RestaurantDbContext _dbContext;

    public MenuItemsService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MenuItem>>> GetMenuItemsAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        IQueryable<MenuItem> queryable = _dbContext.MenuItems.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            queryable = queryable.Where(mi => mi.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<Result<MenuItem>> GetMenuItemAsync(Guid menuItemId, CancellationToken cancellationToken = default)
    {
        var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(mi => mi.Id == menuItemId, cancellationToken);

        if (menuItem is null)
            return Result.NotFound();

        return Result.Success(menuItem);
    }

    public async Task<Result<MenuItem>> CreateMenuItemAsync(CreateMenuItemDTO createMenuItemDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (await _dbContext.MenuItems.AnyAsync(mi => mi.Name == createMenuItemDTO.Name, cancellationToken))
                return Result.Conflict();

            var menuCategory = await _dbContext.MenuCategories.FirstOrDefaultAsync(mc => mc.Id == createMenuItemDTO.CategoryId, cancellationToken);

            if (menuCategory is null)
                return Result.Error();

            var menuItem = new MenuItem(
                Guid.NewGuid(),
                createMenuItemDTO.Name,
                createMenuItemDTO.Description,
                string.Empty,
                createMenuItemDTO.Price,
                menuCategory
            );

            await _dbContext.MenuItems.AddAsync(menuItem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Created(menuItem);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result<MenuItem>> UpdateMenuItemAsync(Guid menuItemId, UpdateMenuItemDTO updateMenuItemDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(mi => mi.Id == menuItemId, cancellationToken);

            if (menuItem is null)
                return Result.NotFound();

            if (menuItem.Category.Id == updateMenuItemDTO.CategoryId)
            {
                var menuCategory = await _dbContext.MenuCategories.FirstOrDefaultAsync(mc => mc.Id == updateMenuItemDTO.CategoryId, cancellationToken);

                if (menuCategory is null)
                    return Result.Error();

                menuItem.ChangeCategory(menuCategory);
            }

            menuItem.ChangeName(updateMenuItemDTO.Name);
            menuItem.ChangeDescription(updateMenuItemDTO.Description);
            menuItem.ChangePrice(updateMenuItemDTO.Price.GetValueOrDefault());

            _dbContext.MenuItems.Update(menuItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Success(menuItem);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result> RemoveMenuItem(Guid menuItemId, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(mi => mi.Id == menuItemId, cancellationToken);

            if (menuItem is null)
                return Result.NotFound();

            menuItem.MarkDeleted();

            _dbContext.MenuItems.Update(menuItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

}
