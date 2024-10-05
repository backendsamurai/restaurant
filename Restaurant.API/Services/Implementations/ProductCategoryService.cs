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
    private readonly IRepository<ProductCategory> _productCategoryRepository = productCategoryRepository;
    private readonly IRedisCollection<ProductCategoryModel> _cache = cache;
    private readonly IValidator<CreateProductCategoryModel> _createProductValidator = createProductValidator;
    private readonly IValidator<UpdateProductCategoryModel> _updateProductValidator = updateProductValidator;

    public async Task<Result<List<ProductCategory>>> GetProductCategories() =>
        await _cache.GetOrSetAsync(_productCategoryRepository.SelectAllAsync);

    public async Task<Result<ProductCategory>> GetProductCategoryById(Guid id)
    {
        var category = await _cache.GetOrSetAsync(pc => pc.Id == id,
            async () => await _productCategoryRepository.SelectByIdAsync(id));

        return category is null
            ? Result.NotFound(
                code: "PRC-000-001",
                type: "entity_not_found",
                message: "Category not found",
                detail: "Please provide correct category id"
            ) : Result.Success(category);
    }

    public async Task<Result<List<ProductCategory>>> GetProductCategoriesByName(string name)
    {
        var categories = await _cache.GetOrSetAsync(
            pc => pc.Name.Contains(name),
            async () => await _productCategoryRepository.WhereAsync<ProductCategory>(pc => pc.Name.Contains(name))
        );

        return Result.Success(categories);
    }

    public async Task<Result<ProductCategory>> CreateProductCategory(CreateProductCategoryModel createProductCategoryModel)
    {
        var validationResult = await _createProductValidator.ValidateAsync(createProductCategoryModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "RPC-000-002",
                type: "invalid_model",
                message: "One of provided field are invalid",
                detail: "Check all fields is valid and try again"
            );

        var category = await _productCategoryRepository.FirstOrDefaultAsync(pc => pc.Name == createProductCategoryModel.Name!);

        if (category is not null)
            return Result.Conflict(
                code: "RPC-100-001",
                type: "conflict_entities",
                message: "Category with this name already exists",
                detail: "Change provided name to another and try again"
            );

        var newCategory = await _productCategoryRepository
            .AddAsync(new ProductCategory { Name = createProductCategoryModel.Name });

        if (newCategory is not null)
        {
            await _cache.InsertAsync(newCategory);
            return Result.Created(newCategory);
        }

        return Result.Error(
            code: "RPC-100-002",
            type: "error_while_creating_entity",
            message: "Error while creating category",
            detail: "Try again later"
        );
    }

    public async Task<Result<ProductCategory>> UpdateProductCategory(Guid id, UpdateProductCategoryModel updateProductCategoryModel)
    {
        var validationResult = await _updateProductValidator.ValidateAsync(updateProductCategoryModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "RPC-000-002",
                type: "invalid_model",
                message: "One of provided field are invalid",
                detail: "Check all fields is valid and try again"
            );

        var category = await _productCategoryRepository.SelectByIdAsync(id);

        if (category is null)
            return Result.NotFound(
                 code: "PRC-000-001",
                 type: "entity_not_found",
                 message: "Category not found",
                 detail: "Please provide correct category id"
             );

        if (category.Name == updateProductCategoryModel.Name)
            return Result.NoContent();

        category.Name = updateProductCategoryModel.Name;

        if (await _productCategoryRepository.UpdateAsync(category))
        {
            await _cache.UpdateAsync(category);
            return Result.Success(category);
        }

        return Result.Error(
            code: "RPC-100-003",
            type: "error_while_updating_entity",
            message: "Error while updating category",
            detail: "Try again later"
        );
    }

    public async Task<Result> RemoveProductCategory(Guid id)
    {
        var category = await _productCategoryRepository.SelectByIdAsync(id);

        if (category is null)
            return Result.NotFound(
                 code: "PRC-000-001",
                 type: "entity_not_found",
                 message: "Category not found",
                 detail: "Please provide correct category id"
             );

        if (await _productCategoryRepository.RemoveAsync(category))
        {
            await _cache.DeleteAsync(category);
            return Result.NoContent();
        }

        return Result.Error(
            code: "RPC-100-004",
            type: "error_while_removing_entity",
            message: "Error while removing category",
            detail: "Try again later"
        );
    }
}
