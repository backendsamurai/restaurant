using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.GetProductsByName;

public sealed class GetProductsByNameQuery : IRequest<Result<List<Domain.Product>>>
{
    public required string ProductName { get; set; }
}