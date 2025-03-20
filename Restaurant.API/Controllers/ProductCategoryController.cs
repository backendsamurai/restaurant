using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.Application.ProductCategory.CreateProductCategory;
using Restaurant.Application.ProductCategory.GetProductCategories;
using Restaurant.Application.ProductCategory.GetProductCategoriesByName;
using Restaurant.Application.ProductCategory.GetProductCategoryById;
using Restaurant.Application.ProductCategory.RemoveProductCategory;
using Restaurant.Application.ProductCategory.UpdateProductCategory;
using Restaurant.Domain;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.ProductCategory;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("products/categories")]
public sealed class ProductCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<ProductCategory>>> GetAllCategories([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await mediator.Send(new GetProductCategoriesByNameQuery { CategoryName = name });
        }

        return await mediator.Send(new GetProductCategoriesQuery());
    }

    [HttpGet("{id:guid}")]
    public async Task<Result<ProductCategory>> GetCategoryById([FromRoute(Name = "id")] Guid id)
        => await mediator.Send(new GetProductCategoryByIdQuery { CategoryId = id });

    [HttpPost]
    public async Task<Result<ProductCategory>> CreateCategory([FromBody] CreateProductCategoryCommand command)
        => await mediator.Send(command);

    [HttpPatch("{id:guid}")]
    public async Task<Result<ProductCategory>> UpdateCategory(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateProductCategoryModel updateProductCategoryModel
    ) => await mediator.Send(new UpdateProductCategoryCommand { CategoryId = id, Name = updateProductCategoryModel.Name });

    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveCategory([FromRoute(Name = "id")] Guid id)
        => await mediator.Send(new RemoveProductCategoryCommand { CategoryId = id });
}
