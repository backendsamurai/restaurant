using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.CreateProductCategory;

public sealed class CreateProductCategoryCommand : IRequest<Result<Domain.ProductCategory>>
{
    public required string Name { get; set; }
}