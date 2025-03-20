using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.CreateProduct;

public sealed class CreateProductCommand : IRequest<Result<Domain.Product>>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}