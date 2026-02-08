using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.DTOs.Consumer;

namespace Restaurant.Services.Contracts;

public interface IConsumersService
{
    Task<Result<Consumer>> GetConsumerAsync(Guid consumerId, CancellationToken cancellationToken = default);

    Task<Result<Consumer>> CreateConsumerAsync(
        CreateConsumerDTO createConsumerDTO,
        CancellationToken cancellationToken = default);

    Task<Result<Consumer>> UpdateConsumerAsync(
        Guid consumerId, UpdateConsumerDTO updateConsumerDTO,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveConsumerAsync(Guid consumerId, CancellationToken cancellationToken = default);
}
