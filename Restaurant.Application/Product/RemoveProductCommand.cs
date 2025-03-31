using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product;

public sealed record RemoveProductCommand(Guid ProductId) : IRequest<Result>;

public sealed class RemoveProductCommandHandler(IProductService productService) : IRequestHandler<RemoveProductCommand, Result>
{
    public async Task<Result> Handle(RemoveProductCommand request, CancellationToken cancellationToken) =>
        await productService.RemoveProductAsync(request.ProductId);
}