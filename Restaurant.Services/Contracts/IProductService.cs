using Restaurant.Domain;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Product;

namespace Restaurant.Services.Contracts;

public interface IProductService
{
    public Task<Result<List<Product>>> GetAllProductsAsync();
    public Task<Result<Product>> GetProductByIdAsync(Guid id);
    public Task<Result<List<Product>>> GetProductsByNameAsync(string name);
    public Task<Result<Product>> CreateProductAsync(CreateProductModel createProductModel);
    public Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProductModel updateProductModel);
    public Task<Result> RemoveProductAsync(Guid id);
}
