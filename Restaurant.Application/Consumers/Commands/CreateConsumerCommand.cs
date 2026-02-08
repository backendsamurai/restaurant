using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.Consumer;

namespace Restaurant.Application.Consumers.Commands;

public sealed record CreateConsumerCommand(CreateConsumerDTO Dto);

public sealed class CreateConsumerCommandHandler
{
    private readonly IConsumersService _consumersService;

    public CreateConsumerCommandHandler(IConsumersService consumersService)
    {
        _consumersService = consumersService;
    }

    public async Task<Result<Consumer>> HandleAsync(CreateConsumerCommand cmd, CancellationToken cancellationToken) =>
        await _consumersService.CreateConsumerAsync(cmd.Dto, cancellationToken);
}
