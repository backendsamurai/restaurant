using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.RemoveProduct;

public sealed class RemoveProductCommandHandler(IProductService productService) : IRequestHandler<RemoveProductCommand, Result>
{
    public async Task<Result> Handle(RemoveProductCommand request, CancellationToken cancellationToken) =>
        await productService.RemoveProductAsync(request.ProductId);
}