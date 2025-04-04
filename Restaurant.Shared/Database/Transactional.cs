using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Persistence;

namespace Restaurant.Shared.Database;

public sealed class Transactional(RestaurantDbContext context, ILogger<Transactional> logger) : ITransactional
{
    private readonly RestaurantDbContext _context = context;
    private readonly ILogger<Transactional> _logger = logger;

    public T? UseTransaction<T>(Func<DbContext, T?> callback, Func<T?> errorCallback)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            T? value = callback(_context);

            _logger.LogInformation("Commit transaction");
            transaction.Commit();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            transaction.Rollback();

            return errorCallback();
        };
    }

    public T? UseTransaction<T>(Func<DbContext, T?> callback)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            T? value = callback(_context);

            _logger.LogInformation("Commit transaction");
            transaction.Commit();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            transaction.Rollback();

            return default;
        };
    }

    public T? UseTransaction<T>(Func<DbContext, T?> callback, T? valueWhenError)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            T? value = callback(_context);

            _logger.LogInformation("Commit transaction");
            transaction.Commit();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            transaction.Rollback();

            return valueWhenError ?? default;
        };
    }

    public async Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback, Func<Task<T?>> errorCallback)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            T? value = await callback(_context);

            _logger.LogInformation("Commit transaction");
            await transaction.CommitAsync();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            await transaction.RollbackAsync();

            return await errorCallback();
        };
    }

    public async Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            T? value = await callback(_context);

            _logger.LogInformation("Commit transaction");
            await transaction.CommitAsync();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            await transaction.RollbackAsync();

            return default;
        };
    }

    public async Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback, T? valueWhenError)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            T? value = await callback(_context);

            _logger.LogInformation("Commit transaction");
            await transaction.CommitAsync();

            return value;
        }
        catch (Exception e)
        {
            _logger.LogError("Rollback transaction\n{@ErrorMsg}", e.Message);
            await transaction.RollbackAsync();

            return valueWhenError ?? default;
        };
    }
}