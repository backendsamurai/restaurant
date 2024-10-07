using Microsoft.EntityFrameworkCore;

namespace Restaurant.API.Types;

public interface ITransactional
{
    public T? UseTransaction<T>(Func<DbContext, T?> callback, Func<T?> errorCallback);
    public T? UseTransaction<T>(Func<DbContext, T?> callback);
    public T? UseTransaction<T>(Func<DbContext, T?> callback, T? valueWhenError);
    public Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback, Func<Task<T?>> errorCallback);
    public Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback);
    public Task<T?> UseTransactionAsync<T>(Func<DbContext, Task<T?>> callback, T? valueWhenError);
}