using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Product;

namespace Restaurant.Application.Product;

public sealed record CreateProductCommand(string Name, string Description, decimal Price, Guid CategoryId) : IRequest<Result<Domain.Product>>;

public sealed class CreateProductCommandHandler(IProductService productService) : IRequestHandler<CreateProductCommand, Result<Domain.Product>>
{
    public async Task<Result<Domain.Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken) =>
        await productService.CreateProductAsync(
            new CreateProductModel(request.Name, request.Description, request.Price, request.CategoryId)
        );
}