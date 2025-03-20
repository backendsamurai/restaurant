using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Product.GetProducts;

public sealed class GetProductsQuery : IRequest<Result<List<Domain.Product>>> { }