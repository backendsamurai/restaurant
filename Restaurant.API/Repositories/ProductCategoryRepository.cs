using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class ProductCategoryRepository(
    RestaurantDbContext context
) : IProductCategoryRepository
{
    private readonly RestaurantDbContext _context = context;

    public async Task<List<ProductCategory>> SelectAllAsync() =>
        await _context.ProductCategories.ToListAsync();

    public async Task<ProductCategory?> SelectByIdAsync(Guid id) =>
        await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id);

    public IQueryable<ProductCategory> SelectByName(string name) =>
        _context.ProductCategories
            .Where(pc => pc.Name.Contains(name))
            .AsNoTracking()
            .AsQueryable();

    public async Task<ProductCategory?> AddAsync(ProductCategory category)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.ProductCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return category;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(ProductCategory category)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.ProductCategories.Update(category);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> RemoveAsync(ProductCategory category)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
