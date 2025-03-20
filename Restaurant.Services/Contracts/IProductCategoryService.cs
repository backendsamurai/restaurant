using Restaurant.Domain;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.ProductCategory;

namespace Restaurant.Services.Contracts;

public interface IProductCategoryService
{
    public Task<Result<List<ProductCategory>>> GetProductCategories();
    public Task<Result<ProductCategory>> GetProductCategoryById(Guid id);
    public Task<Result<List<ProductCategory>>> GetProductCategoriesByName(string name);
    public Task<Result<ProductCategory>> CreateProductCategory(CreateProductCategoryModel createProductCategoryModel);
    public Task<Result<ProductCategory>> UpdateProductCategory(Guid id, UpdateProductCategoryModel updateProductCategoryModel);
    public Task<Result> RemoveProductCategory(Guid id);
}
