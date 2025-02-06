using FluentValidation;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.ProductCategory;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class ProductCategoryService(
    IRepository<ProductCategory> productCategoryRepository,
    IRedisCollection<ProductCategoryModel> cache,
    IValidator<CreateProductCategoryModel> createProductValidator,
    IValidator<UpdateProductCategoryModel> updateProductValidator
) : IProductCategoryService
{
    public async Task<Result<List<ProductCategory>>> GetProductCategories() =>
        await cache.GetOrSetAsync(productCategoryRepository.SelectAllAsync);

    public async Task<Result<ProductCategory>> GetProductCategoryById(Guid id)
    {
        var category = await cache.GetOrSetAsync(pc => pc.Id == id,
            async () => await productCategoryRepository.SelectByIdAsync(id));

        return category is null ? DetailedError.NotFound("Please provide correct category id") : Result.Success(category);
    }

    public async Task<Result<List<ProductCategory>>> GetProductCategoriesByName(string name)
    {
        var categories = await cache.GetOrSetAsync(
            pc => pc.Name.Contains(name),
            async () => await productCategoryRepository.WhereAsync<ProductCategory>(pc => pc.Name.Contains(name))
        );

        return Result.Success(categories);
    }

    public async Task<Result<ProductCategory>> CreateProductCategory(CreateProductCategoryModel createProductCategoryModel)
    {
        var validationResult = await createProductValidator.ValidateAsync(createProductCategoryModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var category = await productCategoryRepository.FirstOrDefaultAsync(pc => pc.Name == createProductCategoryModel.Name!);

        if (category is not null)
            return DetailedError.Conflict("Category with this name already exists", "Category with this name already exists");

        var newCategory = await productCategoryRepository
            .AddAsync(new ProductCategory { Name = createProductCategoryModel.Name });

        if (newCategory is not null)
        {
            await cache.InsertAsync(newCategory);
            return Result.Created(newCategory);
        }

        return DetailedError.CreatingProblem("Error while creating category", "Try again later");
    }

    public async Task<Result<ProductCategory>> UpdateProductCategory(Guid id, UpdateProductCategoryModel updateProductCategoryModel)
    {
        var validationResult = await updateProductValidator.ValidateAsync(updateProductCategoryModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var category = await productCategoryRepository.SelectByIdAsync(id);

        if (category is null)
            return DetailedError.NotFound("Please provide correct category id");

        if (category.Name == updateProductCategoryModel.Name)
            return Result.NoContent();

        category.Name = updateProductCategoryModel.Name;

        if (await productCategoryRepository.UpdateAsync(category))
        {
            await cache.UpdateAsync(category);
            return Result.Success(category);
        }

        return DetailedError.UpdatingProblem("Error while updating category", "Try again later");
    }

    public async Task<Result> RemoveProductCategory(Guid id)
    {
        var category = await productCategoryRepository.SelectByIdAsync(id);

        if (category is null)
            return DetailedError.NotFound("Please provide correct category id");

        if (await productCategoryRepository.RemoveAsync(category))
        {
            await cache.DeleteAsync(category);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Error while removing category", "Try again later");
    }
}
