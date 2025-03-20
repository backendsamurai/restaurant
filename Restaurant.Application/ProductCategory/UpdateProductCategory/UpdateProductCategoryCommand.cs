using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.UpdateProductCategory;

public sealed class UpdateProductCategoryCommand : IRequest<Result<Domain.ProductCategory>>
{
    public Guid CategoryId { get; set; }
    public required string Name { get; set; }
}
