using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;
using Restaurant.Persistence;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.Consumer;

namespace Restaurant.Services.Implementations;

public sealed class ConsumersService : IConsumersService
{
    private readonly RestaurantDbContext _dbContext;

    private readonly IPasswordHasherService _passwordHasherService;

    public ConsumersService(RestaurantDbContext dbContext, IPasswordHasherService passwordHasherService)
    {
        _dbContext = dbContext;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result<Consumer>> GetConsumerAsync(Guid consumerId, CancellationToken cancellationToken = default)
    {
        var consumer = await _dbContext.Consumers.FirstOrDefaultAsync(c => c.Id == consumerId, cancellationToken);

        if (consumer is null)
            return Result.NotFound();

        return Result.Success(consumer);
    }

    public async Task<Result<Consumer>> CreateConsumerAsync(CreateConsumerDTO createConsumerDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (await _dbContext.Consumers.AnyAsync(c => c.Email == createConsumerDTO.Email, cancellationToken))
                return Result.Conflict();

            var hashedPassword = _passwordHasherService.Hash(createConsumerDTO.Password);

            var consumer = new Consumer(
                Guid.NewGuid(), createConsumerDTO.Name,
                createConsumerDTO.Email, hashedPassword);

            await _dbContext.Consumers.AddAsync(consumer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Created(consumer);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result<Consumer>> UpdateConsumerAsync(Guid consumerId, UpdateConsumerDTO updateConsumerDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var consumer = await _dbContext.Consumers.FirstOrDefaultAsync(c => c.Id == consumerId, cancellationToken);

            if (consumer is null)
                return Result.NotFound();

            consumer.ChangeName(updateConsumerDTO.Name);
            consumer.ChangeEmail(updateConsumerDTO.Email);

            if (!string.IsNullOrWhiteSpace(updateConsumerDTO.Password))
            {
                var passwordHash = _passwordHasherService.Hash(updateConsumerDTO.Password);
                consumer.ChangePasswordHash(passwordHash);
            }

            _dbContext.Consumers.Update(consumer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return Result.Success(consumer);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result> RemoveConsumerAsync(Guid consumerId, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var consumer = await _dbContext.Consumers.FirstOrDefaultAsync(c => c.Id == consumerId, cancellationToken);

            if (consumer is null)
                return Result.NotFound();

            consumer.MarkDeleted();

            _dbContext.Update(consumer);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }
}
