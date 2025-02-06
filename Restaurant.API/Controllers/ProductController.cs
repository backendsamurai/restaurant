using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Models.Product;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Controllers;

[ApiController]
[Authorize(AuthorizationPolicies.RequireAdmin)]
[Route("products")]
public sealed class ProductController(IProductService productService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<Result<List<Product>>> GetAllProducts([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await productService.GetProductsByNameAsync(name);
        }

        return await productService.GetAllProductsAsync();
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<Result<Product>> GetProductById([FromRoute(Name = "id")] Guid id) =>
        await productService.GetProductByIdAsync(id);

    [HttpPost]
    public async Task<Result<Product>> CreateProduct([FromBody] CreateProductModel createProductModel) =>
        await productService.CreateProductAsync(createProductModel);

    [HttpPatch("{id:guid}")]
    public async Task<Result<Product>> UpdateProduct(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductModel updateProductModel
    ) => await productService.UpdateProductAsync(id, updateProductModel);

    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveProduct([FromRoute(Name = "id")] Guid id) =>
        await productService.RemoveProductAsync(id);
}
