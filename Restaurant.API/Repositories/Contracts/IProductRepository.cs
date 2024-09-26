using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface IProductRepository
{
    public Task<List<Product>> SelectAllProductsAsync();
    public Task<Product?> SelectProductByIdAsync(Guid id);
    public IQueryable<Product> SelectProductsByName(string name);
    public Task<Product?> AddAsync(Product product);
    public Task<bool> UpdateAsync(Product product);
    public Task<bool> RemoveAsync(Product product);
}
