using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Entities;
using Restaurant.API.Models.Product;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Authorize(AuthorizationPolicies.RequireEmployeeManager)]
[Route("products")]
public sealed class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [AllowAnonymous]
    [ApplyResult]
    [HttpGet]
    public async Task<Result<List<Product>>> GetAllProducts([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return Result.Invalid(validationResult.DetailedError!);

            return await _productService.GetProductsByNameAsync(name);
        }

        return await _productService.GetAllProductsAsync();
    }

    [AllowAnonymous]
    [ApplyResult]
    [HttpGet("{id:guid}")]
    public async Task<Result<Product>> GetProductById([FromRoute(Name = "id")] Guid id) =>
        await _productService.GetProductByIdAsync(id);

    [ApplyResult]
    [HttpPost]
    public async Task<Result<Product>> CreateProduct([FromBody] CreateProductModel createProductModel) =>
        await _productService.CreateProductAsync(createProductModel);

    [ApplyResult]
    [HttpPatch("{id:guid}")]
    public async Task<Result<Product>> UpdateProduct(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductModel updateProductModel
    ) => await _productService.UpdateProductAsync(id, updateProductModel);

    [ApplyResult]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveProduct([FromRoute(Name = "id")] Guid id) =>
        await _productService.RemoveProductAsync(id);
}
