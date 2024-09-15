using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Entities;
using Restaurant.API.Models.ProductCategory;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("products/categories")]
public sealed class ProductCategoryController(
    IProductCategoryService productCategoryService
) : ControllerBase
{
    private readonly IProductCategoryService _productCategoryService = productCategoryService;

    [ApplyResult]
    [HttpGet]
    public async Task<Result<List<ProductCategory>>> GetAllCategories([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return Result.Invalid(validationResult.DetailedError!);

            return await _productCategoryService.GetProductCategoriesByName(name);
        }

        return await _productCategoryService.GetProductCategories();
    }

    [ApplyResult]
    [HttpGet("{id:guid}")]
    public async Task<Result<ProductCategory>> GetCategoryById([FromRoute(Name = "id")] Guid id)
        => await _productCategoryService.GetProductCategoryById(id);

    [ApplyResult]
    [HttpPost]
    public async Task<Result<ProductCategory>> CreateCategory([FromBody] CreateProductCategoryModel createProductCategoryModel)
        => await _productCategoryService.CreateProductCategory(createProductCategoryModel);

    [ApplyResult]
    [HttpPatch("{id:guid}")]
    public async Task<Result<ProductCategory>> UpdateCategory(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductCategoryModel updateProductCategoryModel
    ) => await _productCategoryService.UpdateProductCategory(id, updateProductCategoryModel);

    [ApplyResult]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveCategory([FromRoute(Name = "id")] Guid id)
        => await _productCategoryService.RemoveProductCategory(id);
}
