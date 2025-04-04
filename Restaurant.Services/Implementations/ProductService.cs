using FluentValidation;
using Redis.OM.Searching;
using Restaurant.Domain;
using Restaurant.Infrastructure.Cache.Models;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Database;
using Restaurant.Shared.Extensions;
using Restaurant.Shared.Models.Product;

namespace Restaurant.Services.Implementations;

public sealed class ProductService(
    IRepository<Product> productRepository,
    IRepository<ProductCategory> productCategoryRepository,
    IValidator<CreateProductModel> createProductModelValidator,
    IValidator<UpdateProductModel> updateProductModelValidator,
    IRedisCollection<ProductCache> cache
) : IProductService
{
    public async Task<Result<List<Product>>> GetAllProductsAsync() =>
        await cache.GetOrSetAsync(productRepository.SelectAllAsync);

    public async Task<Result<Product>> GetProductByIdAsync(Guid id)
    {
        var product = await cache.GetOrSetAsync(
            p => p.Id == id, async () => await productRepository.SelectByIdAsync(id));

        return product is null ? DetailedError.NotFound("Provide correct product ID") : product;
    }

    public async Task<Result<List<Product>>> GetProductsByNameAsync(string name) =>
        await cache.GetOrSetAsync(p => p.Name.Contains(name),
            async () => await productRepository.WhereAsync<Product>(p => p.Name.Contains(name)));

    public async Task<Result<Product>> CreateProductAsync(CreateProductModel createProductModel)
    {
        var validationResult = await createProductModelValidator.ValidateAsync(createProductModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field is invalid", "Please provide correct data and try again");

        var productFromDb = await productRepository.FirstOrDefaultAsync(p => p.Name == createProductModel.Name);

        if (productFromDb is not null)
            return DetailedError.Conflict("Product with this name already exists", "Please check provided name or provide another name of product");

        var productCategory = await productCategoryRepository.SelectByIdAsync(createProductModel.CategoryId);

        if (productCategory is null)
            return DetailedError.NotFound("Category of product not found", "Please provide correct category id and try again");

        var product = await productRepository.AddAsync(
            Product.Create(Guid.NewGuid(), createProductModel.Name, createProductModel.Description, "", createProductModel.Price, productCategory)
        );

        if (product is not null)
            return Result.Created(product);

        return DetailedError.CreatingProblem("Cannot create product", "Unexpected error");
    }

    public async Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProductModel updateProductModel)
    {
        bool isModified = false;

        var product = await productRepository.SelectByIdAsync(id);

        if (product is null)
            return DetailedError.NotFound("Provide correct product ID");

        if (updateProductModel.Name is not null && updateProductModel.Name != product.Name)
        {
            var validationResult = await updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Name));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid name", validationResult.Errors.First().ErrorMessage);

            product.ChangeName(updateProductModel.Name);
            isModified = true;
        }

        if (updateProductModel.Description is not null && updateProductModel.Description != product.Description)
        {
            var validationResult = await updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Description));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid description", validationResult.Errors.First().ErrorMessage);

            product.ChangeDescription(updateProductModel.Description);
            isModified = true;
        }

        if (updateProductModel.Price is not null && updateProductModel.Price != product.Price)
        {
            var validationResult = await updateProductModelValidator.ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Price));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid price", validationResult.Errors.First().ErrorMessage);

            product.ChangePrice((decimal)updateProductModel.Price);
            isModified = true;
        }

        if (updateProductModel.CategoryId is not null && updateProductModel.CategoryId != product.Category.Id)
        {
            var category = await productCategoryRepository.SelectByIdAsync((Guid)updateProductModel.CategoryId);

            if (category is null)
                return DetailedError.NotFound("Product category not found", "Provide correct category id");

            product.ChangeCategory(category);
            isModified = true;
        }

        if (isModified)
        {
            if (await productRepository.UpdateAsync(product))
            {
                await cache.UpdateAsync(product);
                return Result.Success(product);
            }

            return DetailedError.UpdatingProblem("Cannot update product", "Unexpected error");
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveProductAsync(Guid id)
    {
        var product = await productRepository.SelectByIdAsync(id);

        if (product is null)
            return DetailedError.NotFound("Provide correct product ID");

        var isRemoved = await productRepository.RemoveAsync(product);

        if (isRemoved)
        {
            await cache.DeleteAsync(product);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Cannot remove product", "Unexpected error");
    }
}
