using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.RemoveProductCategory;

public sealed class RemoveProductCategoryCommandHandler(IProductCategoryService productCategoryService) : IRequestHandler<RemoveProductCategoryCommand, Result>
{
    public async Task<Result> Handle(RemoveProductCategoryCommand request, CancellationToken cancellationToken) =>
       await productCategoryService.RemoveProductCategory(request.CategoryId);
}