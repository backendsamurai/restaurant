using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product;

public sealed record GetProductsQuery : IRequest<Result<List<Domain.Product>>>;

public sealed class GetProductsQueryHandler(IProductService productService) : IRequestHandler<GetProductsQuery, Result<List<Domain.Product>>>
{
    public async Task<Result<List<Domain.Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        await productService.GetAllProductsAsync();
}
