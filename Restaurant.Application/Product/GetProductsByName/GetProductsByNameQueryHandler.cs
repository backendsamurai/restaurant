using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.GetProductsByName;

public sealed class GetProductsByNameQueryHandler(IProductService productService) : IRequestHandler<GetProductsByNameQuery, Result<List<Domain.Product>>>
{
    public async Task<Result<List<Domain.Product>>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken) =>
        await productService.GetProductsByNameAsync(request.ProductName);
}
