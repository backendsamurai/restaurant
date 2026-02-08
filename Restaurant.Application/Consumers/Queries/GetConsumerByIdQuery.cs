using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.Consumers.Queries;

public sealed record GetConsumerByIdQuery(Guid ConsumerId);

public sealed class GetConsumerByIdQueryHandler
{
    private readonly IConsumersService _consumersService;

    public GetConsumerByIdQueryHandler(IConsumersService consumersService)
    {
        _consumersService = consumersService;
    }

    public async Task<Result<Consumer>> HandleAsync(GetConsumerByIdQuery query, CancellationToken cancellationToken) =>
       await _consumersService.GetConsumerAsync(query.ConsumerId, cancellationToken);
}
