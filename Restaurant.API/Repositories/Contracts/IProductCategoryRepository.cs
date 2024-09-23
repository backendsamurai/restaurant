using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface IProductCategoryRepository
{
    public Task<List<ProductCategory>> SelectAllAsync();
    public IQueryable<ProductCategory> SelectByName(string name);
    public Task<ProductCategory?> SelectByIdAsync(Guid id);
    public Task<ProductCategory?> AddAsync(ProductCategory category);
    public Task<bool> UpdateAsync(ProductCategory category);
    public Task<bool> RemoveAsync(ProductCategory category);
}
