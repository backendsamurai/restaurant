using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.GetProductCategoriesByName;

public sealed class GetProductCategoriesByNameQuery : IRequest<Result<List<Domain.ProductCategory>>>
{
    public required string CategoryName { get; set; }
}