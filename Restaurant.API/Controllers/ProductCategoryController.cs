using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Models.ProductCategory;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("products/categories")]
public sealed class ProductCategoryController(
    IProductCategoryService productCategoryService
) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<ProductCategory>>> GetAllCategories([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await productCategoryService.GetProductCategoriesByName(name);
        }

        return await productCategoryService.GetProductCategories();
    }

    [HttpGet("{id:guid}")]
    public async Task<Result<ProductCategory>> GetCategoryById([FromRoute(Name = "id")] Guid id)
        => await productCategoryService.GetProductCategoryById(id);

    [HttpPost]
    public async Task<Result<ProductCategory>> CreateCategory([FromBody] CreateProductCategoryModel createProductCategoryModel)
        => await productCategoryService.CreateProductCategory(createProductCategoryModel);

    [HttpPatch("{id:guid}")]
    public async Task<Result<ProductCategory>> UpdateCategory(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductCategoryModel updateProductCategoryModel
    ) => await productCategoryService.UpdateProductCategory(id, updateProductCategoryModel);

    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveCategory([FromRoute(Name = "id")] Guid id)
        => await productCategoryService.RemoveProductCategory(id);
}
