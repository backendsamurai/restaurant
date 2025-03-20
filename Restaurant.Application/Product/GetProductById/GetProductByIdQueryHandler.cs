using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductService productService) : IRequestHandler<GetProductByIdQuery, Result<Domain.Product>>
{
    public async Task<Result<Domain.Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) =>
        await productService.GetProductByIdAsync(request.ProductId);
}