using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory;

public sealed record GetProductCategoriesByNameQuery(string CategoryName) : IRequest<Result<List<Domain.ProductCategory>>>;

public sealed class GetProductCategoriesByNameQueryHandler(IProductCategoryService productCategoryService) : IRequestHandler<GetProductCategoriesByNameQuery, Result<List<Domain.ProductCategory>>>
{
    public async Task<Result<List<Domain.ProductCategory>>> Handle(GetProductCategoriesByNameQuery request, CancellationToken cancellationToken) =>
        await productCategoryService.GetProductCategoriesByName(request.CategoryName);
}