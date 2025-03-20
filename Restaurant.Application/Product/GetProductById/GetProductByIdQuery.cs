using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.GetProductById;

public sealed class GetProductByIdQuery : IRequest<Result<Domain.Product>>
{
    public Guid ProductId { get; set; }
}