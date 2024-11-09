using FluentValidation;
using Mapster;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Product;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class ProductService(
    IRepository<Product> productRepository,
    IRepository<ProductCategory> productCategoryRepository,
    IValidator<CreateProductModel> createProductModelValidator,
    IValidator<UpdateProductModel> updateProductModelValidator,
    IRedisCollection<ProductModel> cache
) : IProductService
{
    private readonly IRepository<Product> _productRepository = productRepository;
    private readonly IRepository<ProductCategory> _productCategoryRepository = productCategoryRepository;
    private readonly IValidator<CreateProductModel> _createProductModelValidator = createProductModelValidator;
    private readonly IValidator<UpdateProductModel> _updateProductModelValidator = updateProductModelValidator;
    private readonly IRedisCollection<ProductModel> _cache = cache;

    public async Task<Result<List<Product>>> GetAllProductsAsync() =>
        await _cache.GetOrSetAsync(_productRepository.SelectAllAsync);

    public async Task<Result<Product>> GetProductByIdAsync(Guid id)
    {
        var product = await _cache.GetOrSetAsync(
            p => p.Id == id, async () => await _productRepository.SelectByIdAsync(id));

        return product is null ? DetailedError.NotFound("Provide correct product ID") : product;
    }

    public async Task<Result<List<Product>>> GetProductsByNameAsync(string name) =>
        await _cache.GetOrSetAsync(p => p.Name.Contains(name),
            async () => await _productRepository.WhereAsync<Product>(p => p.Name.Contains(name)));

    public async Task<Result<Product>> CreateProductAsync(CreateProductModel createProductModel)
    {
        var validationResult = await _createProductModelValidator.ValidateAsync(createProductModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field is invalid", "Please provide correct data and try again");

        var productFromDb = await _productRepository.FirstOrDefaultAsync(p => p.Name == createProductModel.Name);

        if (productFromDb is not null)
            return DetailedError.Conflict("Product with this name already exists", "Please check provided name or provide another name of product");

        var productCategory = await _productCategoryRepository.SelectByIdAsync(createProductModel.CategoryId);

        if (productCategory is null)
            return DetailedError.NotFound("Category of product not found", "Please provide correct category id and try again");

        var product = createProductModel.Adapt<Product>();

        product.Category = productCategory;

        var createdProduct = await _productRepository.AddAsync(product);

        if (createdProduct is not null)
            return Result.Created(createdProduct);

        return DetailedError.CreatingProblem("Cannot create product", "Unexpected error");
    }

    public async Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProductModel updateProductModel)
    {
        bool isModified = false;

        var product = await _productRepository.SelectByIdAsync(id);

        if (product is null)
            return DetailedError.NotFound("Provide correct product ID");

        if (updateProductModel.Name is not null && updateProductModel.Name != product.Name)
        {
            var validationResult = await _updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Name));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid name", validationResult.Errors.First().ErrorMessage);

            product.Name = updateProductModel.Name;
            isModified = true;
        }

        if (updateProductModel.Description is not null && updateProductModel.Description != product.Description)
        {
            var validationResult = await _updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Description));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid description", validationResult.Errors.First().ErrorMessage);

            product.Description = updateProductModel.Description;
            isModified = true;
        }

        if (updateProductModel.Price is not null && updateProductModel.Price != product.Price)
        {
            var validationResult = await _updateProductModelValidator.ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Price));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid price", validationResult.Errors.First().ErrorMessage);

            product.OldPrice = product.Price;
            product.Price = (decimal)updateProductModel.Price;
            isModified = true;
        }

        if (updateProductModel.CategoryId is not null && updateProductModel.CategoryId != product.Category.Id)
        {
            var category = await _productCategoryRepository.SelectByIdAsync((Guid)updateProductModel.CategoryId);

            if (category is null)
                return DetailedError.NotFound("Product category not found", "Provide correct category id");

            product.Category = category;
            isModified = true;
        }

        if (isModified)
        {
            if (await _productRepository.UpdateAsync(product))
            {
                await _cache.UpdateAsync(product);
                return Result.Success(product);
            }

            return DetailedError.UpdatingProblem("Cannot update product", "Unexpected error");
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveProductAsync(Guid id)
    {
        var product = await _productRepository.SelectByIdAsync(id);

        if (product is null)
            return DetailedError.NotFound("Provide correct product ID");

        var isRemoved = await _productRepository.RemoveAsync(product);

        if (isRemoved)
        {
            await _cache.DeleteAsync(product);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Cannot remove product", "Unexpected error");
    }
}
