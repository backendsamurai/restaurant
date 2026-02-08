using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.Consumer;

namespace Restaurant.Application.Consumers.Commands;

public sealed record UpdateConsumerCommand(Guid ConsumerId, UpdateConsumerDTO Dto);

public sealed class UpdateConsumerCommandHandler
{
    private readonly IConsumersService _consumersService;

    public UpdateConsumerCommandHandler(IConsumersService consumersService)
    {
        _consumersService = consumersService;
    }

    public async Task<Result<Consumer>> HandleAsync(UpdateConsumerCommand cmd, CancellationToken cancellationToken) =>
        await _consumersService.UpdateConsumerAsync(cmd.ConsumerId, cmd.Dto, cancellationToken);
}
