using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.RemoveProduct;

public sealed class RemoveProductCommand : IRequest<Result>
{
    public Guid ProductId { get; set; }
}