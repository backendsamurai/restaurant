using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.ProductCategory.GetProductCategories;

public sealed class GetProductCategoriesQuery : IRequest<Result<List<Domain.ProductCategory>>> { }