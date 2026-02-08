using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;
using Restaurant.Persistence;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.MenuCategory;

namespace Restaurant.Services.Implementations;

public sealed class MenuCategoriesService : IMenuCategoriesService
{
    private readonly RestaurantDbContext _dbContext;

    public MenuCategoriesService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MenuCategory>>> GetMenuCategoriesAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        IQueryable<MenuCategory> queryable = _dbContext.MenuCategories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            queryable = queryable.Where(mc => mc.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<Result<MenuCategory>> GetMenuCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default)
    {
        var menuCategory = await _dbContext.MenuCategories.FirstOrDefaultAsync(mc => mc.Id == menuCategoryId, cancellationToken);

        if (menuCategory is null)
            return Result.NotFound();

        return Result.Success(menuCategory);
    }

    public async Task<Result<MenuCategory>> CreateMenuCategory(CreateMenuCategoryDTO createMenuCategoryDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var menuCategory = new MenuCategory(Guid.NewGuid(), createMenuCategoryDTO.Name);

            await _dbContext.MenuCategories.AddAsync(menuCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Created(menuCategory);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result<MenuCategory>> UpdateMenuCategory(Guid menuCategoryId, UpdateMenuCategoryDTO updateMenuCategoryDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var menuCategory = await _dbContext.MenuCategories.FirstOrDefaultAsync(mc => mc.Id == menuCategoryId, cancellationToken);

            if (menuCategory is null)
                return Result.NotFound();


            if (menuCategory.Name == updateMenuCategoryDTO.Name)
                return Result.Success();

            menuCategory.ChangeName(updateMenuCategoryDTO.Name);

            _dbContext.Update(menuCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Success(menuCategory);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result> RemoveMenuCategory(Guid menuCategoryId, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var menuCategory = await _dbContext.MenuCategories.FirstOrDefaultAsync(mc => mc.Id == menuCategoryId, cancellationToken);

            if (menuCategory is null)
                return Result.NotFound();

            menuCategory.MarkDeleted();

            _dbContext.Update(menuCategory);
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
