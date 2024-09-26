using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Product;
using Restaurant.API.Repositories.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class ProductService(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IValidator<CreateProductModel> createProductModelValidator,
    IValidator<UpdateProductModel> updateProductModelValidator,
    IRedisCollection<ProductModel> cache
) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;
    private readonly IValidator<CreateProductModel> _createProductModelValidator = createProductModelValidator;
    private readonly IValidator<UpdateProductModel> _updateProductModelValidator = updateProductModelValidator;
    private readonly IRedisCollection<ProductModel> _cache = cache;

    public async Task<Result<List<Product>>> GetAllProductsAsync() =>
        await _cache.GetOrSetAsync(_productRepository.SelectAllProductsAsync);

    public async Task<Result<Product>> GetProductByIdAsync(Guid id)
    {
        var product = await _cache.GetOrSetAsync(
            p => p.Id == id, async () => await _productRepository.SelectProductByIdAsync(id));

        if (product is null)
            return Result.NotFound(
                code: "PDS-000-001",
                type: "entity_not_found",
                message: "Product not found",
                detail: "Please provide correct id and try again"
            );

        return product;
    }

    public async Task<Result<List<Product>>> GetProductsByNameAsync(string name) =>
        await _cache.GetOrSetAsync(p => p.Name.Contains(name),
            async () => await _productRepository.SelectProductsByName(name).ToListAsync());

    public async Task<Result<Product>> CreateProductAsync(CreateProductModel createProductModel)
    {
        var validationResult = await _createProductModelValidator.ValidateAsync(createProductModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "PDS-000-002",
                type: "invalid_model",
                message: "One of field is invalid",
                detail: "Please provide correct data and try again"
            );

        var productFromDb = await _productRepository.SelectProductsByName(createProductModel.Name).FirstOrDefaultAsync();

        if (productFromDb is not null)
            return Result.Conflict(
                code: "PDS-100-001",
                type: "entity_already_exists",
                message: "Product with this name already exists",
                detail: "Please check provided name or provide another name of product"
            );

        var productCategory = await _productCategoryRepository.SelectByIdAsync(createProductModel.CategoryId);

        if (productCategory is null)
            return Result.NotFound(
                code: "PDS-000-003",
                type: "entity_not_found",
                message: "Category of product not found",
                detail: "Please provide correct category id and try again"
            );

        var product = createProductModel.Adapt<Product>();

        product.Category = productCategory;

        var createdProduct = await _productRepository.AddAsync(product);

        if (createdProduct is not null)
            return Result.Created(createdProduct);

        return Result.Error(
            code: "PDS-100-001",
            type: "error_while_creating_product",
            message: "Cannot create product",
            detail: "Unexpected error"
        );
    }

    public async Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProductModel updateProductModel)
    {
        bool isModified = false;

        var product = await _productRepository.SelectProductByIdAsync(id);

        if (product is null)
            return Result.NotFound(
                code: "PDS-000-001",
                type: "entity_not_found",
                message: "Product not found",
                detail: "Please provide correct id and try again"
            );

        if (updateProductModel.Name is not null && updateProductModel.Name != product.Name)
        {
            var validationResult = await _updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Name));

            if (!validationResult.IsValid)
                return Result.Invalid(
                    code: "PDS-000-002",
                    type: "invalid_model",
                    message: "One of field is invalid",
                    detail: "Please provide correct data and try again"
                );

            product.Name = updateProductModel.Name;
            isModified = true;
        }

        if (updateProductModel.Description is not null && updateProductModel.Description != product.Description)
        {
            var validationResult = await _updateProductModelValidator
                .ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Description));

            if (!validationResult.IsValid)
                return Result.Invalid(
                    code: "PDS-000-002",
                    type: "invalid_model",
                    message: "One of field is invalid",
                    detail: "Please provide correct data and try again"
                );

            product.Description = updateProductModel.Description;
            isModified = true;
        }

        if (updateProductModel.Price is not null && updateProductModel.Price != product.Price)
        {
            var validationResult = await _updateProductModelValidator.ValidateAsync(updateProductModel, opt => opt.IncludeProperties(x => x.Price));

            if (!validationResult.IsValid)
                return Result.Invalid(
                    code: "PDS-000-002",
                    type: "invalid_model",
                    message: "One of field is invalid",
                    detail: "Please provide correct data and try again"
                );

            product.OldPrice = product.Price;
            product.Price = (decimal)updateProductModel.Price;
            isModified = true;
        }

        if (updateProductModel.CategoryId is not null && updateProductModel.CategoryId != product.Category.Id)
        {
            var category = await _productCategoryRepository.SelectByIdAsync((Guid)updateProductModel.CategoryId);

            if (category is null)
                return Result.NotFound(
                    code: "PDS-000-003",
                    type: "entity_not_found",
                    message: "Product category not found",
                    detail: "Provide correct category id"
                );

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

            return Result.Error(
                code: "PDS-100-003",
                type: "error_while_updating_product",
                message: "Cannot update product",
                detail: "Unexpected error"
            );
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveProductAsync(Guid id)
    {
        var product = await _productRepository.SelectProductByIdAsync(id);

        if (product is null)
            return Result.NotFound(
                code: "PDS-000-001",
                type: "entity_not_found",
                message: "Product not found",
                detail: "Please provide correct id"
            );

        var isRemoved = await _productRepository.RemoveAsync(product);

        if (isRemoved)
        {
            await _cache.DeleteAsync(product);
            return Result.NoContent();
        }

        return Result.Error(
            code: "PDS-100-002",
            type: "error_while_removing_product",
            message: "Cannot remove product",
            detail: "Unexpected error"
        );
    }
}
