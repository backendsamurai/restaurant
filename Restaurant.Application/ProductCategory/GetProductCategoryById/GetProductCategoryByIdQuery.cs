using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.GetProductCategoryById;

public sealed class GetProductCategoryByIdQuery : IRequest<Result<Domain.ProductCategory>>
{
    public Guid CategoryId { get; set; }
}