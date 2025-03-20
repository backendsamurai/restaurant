using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.ProductCategory;

namespace Restaurant.Application.ProductCategory.CreateProductCategory;

public sealed class CreateProductCategoryCommandHandler(IProductCategoryService productCategoryService) : IRequestHandler<CreateProductCategoryCommand, Result<Domain.ProductCategory>>
{
    public async Task<Result<Domain.ProductCategory>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken) =>
        await productCategoryService.CreateProductCategory(new CreateProductCategoryModel(request.Name));
}