using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.UpdateProduct;

public sealed class UpdateProductCommand : IRequest<Result<Domain.Product>>
{
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public Guid? CategoryId { get; set; }
}