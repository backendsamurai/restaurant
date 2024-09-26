using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;
using Restaurant.API.Repositories.Contracts;

namespace Restaurant.API.Repositories.Implementations;

public sealed class ProductRepository(RestaurantDbContext context) : IProductRepository
{
    private readonly RestaurantDbContext _context = context;

    public async Task<List<Product>> SelectAllProductsAsync() =>
        await _context.Products.Include(p => p.Category).ToListAsync();

    public async Task<Product?> SelectProductByIdAsync(Guid id) =>
        await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public IQueryable<Product> SelectProductsByName(string name) =>
        _context.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(name))
            .AsNoTracking()
            .AsQueryable();

    public async Task<Product?> AddAsync(Product product)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Attach(product).State = EntityState.Added;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return product;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Attach(product).State = EntityState.Modified;
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

    public async Task<bool> RemoveAsync(Product product)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Products.Remove(product);
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
