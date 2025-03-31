using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory;

public sealed record RemoveProductCategoryCommand(Guid CategoryId) : IRequest<Result>;

public sealed class RemoveProductCategoryCommandHandler(IProductCategoryService productCategoryService) : IRequestHandler<RemoveProductCategoryCommand, Result>
{
    public async Task<Result> Handle(RemoveProductCategoryCommand request, CancellationToken cancellationToken) =>
       await productCategoryService.RemoveProductCategory(request.CategoryId);
}