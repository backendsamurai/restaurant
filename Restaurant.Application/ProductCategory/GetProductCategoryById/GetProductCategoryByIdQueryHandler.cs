using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.GetProductCategoryById;

public sealed class GetProductCategoryByIdQueryHandler(IProductCategoryService productCategoryService) : IRequestHandler<GetProductCategoryByIdQuery, Result<Domain.ProductCategory>>
{
    public async Task<Result<Domain.ProductCategory>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken) =>
        await productCategoryService.GetProductCategoryById(request.CategoryId);
}