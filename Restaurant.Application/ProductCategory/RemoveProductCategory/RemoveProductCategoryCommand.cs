using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.RemoveProductCategory;

public sealed class RemoveProductCategoryCommand : IRequest<Result>
{
    public Guid CategoryId { get; set; }
}