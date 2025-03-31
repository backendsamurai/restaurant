using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.Application.Product;
using Restaurant.Domain;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Product;

namespace Restaurant.API.Controllers;

[ApiController]
[Authorize(AuthorizationPolicies.RequireAdmin)]
[Route("products")]
public sealed class ProductController(IMediator mediator) : ControllerBase
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

            return await mediator.Send(new GetProductsByNameQuery(name));
        }

        return await mediator.Send(new GetProductsQuery());
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<Result<Product>> GetProductById([FromRoute(Name = "id")] Guid id) =>
        await mediator.Send(new GetProductByIdQuery(id));

    [HttpPost]
    public async Task<Result<Product>> CreateProduct([FromBody] CreateProductModel createProductModel) =>
        await mediator.Send(new CreateProductCommand(createProductModel.Name, createProductModel.Description, createProductModel.Price, createProductModel.CategoryId));

    [HttpPatch("{id:guid}")]
    public async Task<Result<Product>> UpdateProduct(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductModel updateProductModel
    ) => await mediator.Send(
        new UpdateProductCommand(id, updateProductModel.Name, updateProductModel.Description, updateProductModel.Price, updateProductModel.CategoryId)
    );

    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveProduct([FromRoute(Name = "id")] Guid id) =>
        await mediator.Send(new RemoveProductCommand(id));
}
