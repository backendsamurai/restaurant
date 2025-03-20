using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.GetProductCategories;

public sealed class GetProductCategoriesQueryHandler(IProductCategoryService productCategoryService) : IRequestHandler<GetProductCategoriesQuery, Result<List<Domain.ProductCategory>>>
{
    public async Task<Result<List<Domain.ProductCategory>>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken) =>
        await productCategoryService.GetProductCategories();
}