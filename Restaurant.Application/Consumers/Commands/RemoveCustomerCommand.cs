using Ardalis.Result;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.Consumers.Commands;

public sealed record RemoveConsumerCommand(Guid ConsumerId);

public sealed class RemoveConsumerCommandHandler
{
    private readonly IConsumersService _consumersService;

    public RemoveConsumerCommandHandler(IConsumersService consumersService)
    {
        _consumersService = consumersService;
    }

    public async Task<Result> HandleAsync(RemoveConsumerCommand cmd, CancellationToken cancellationToken) =>
        await _consumersService.RemoveConsumerAsync(cmd.ConsumerId, cancellationToken);
}
