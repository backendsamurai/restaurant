using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.ProductCategory;

namespace Restaurant.Application.ProductCategory.UpdateProductCategory;

public sealed class UpdateProductCategoryCommandHandler(IProductCategoryService productCategoryService) : IRequestHandler<UpdateProductCategoryCommand, Result<Domain.ProductCategory>>
{
    public async Task<Result<Domain.ProductCategory>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken) =>
        await productCategoryService.UpdateProductCategory(request.CategoryId, new UpdateProductCategoryModel(request.Name));
}
