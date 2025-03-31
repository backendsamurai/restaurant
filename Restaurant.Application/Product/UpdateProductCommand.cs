using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Product;

namespace Restaurant.Application.Product;

public sealed record UpdateProductCommand(Guid ProductId, string? Name, string? Description, decimal? Price, Guid? CategoryId) : IRequest<Result<Domain.Product>>;

public sealed class UpdateProductCommandHandler(IProductService productService) : IRequestHandler<UpdateProductCommand, Result<Domain.Product>>
{
    public async Task<Result<Domain.Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) =>
        await productService.UpdateProductAsync(
            request.ProductId,
            new UpdateProductModel(
                request.Name,
                request.Description,
                request.Price,
                request.CategoryId
            )
        );
}