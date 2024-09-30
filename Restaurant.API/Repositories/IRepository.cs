using System.Linq.Expressions;

namespace Restaurant.API.Repositories;

public interface IRepository<T> where T : class
{
    public List<T> SelectAll();
    public Task<List<T>> SelectAllAsync();
    public T? SelectById(Guid id);
    public Task<T?> SelectByIdAsync(Guid id);
    public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression);
    public IQueryable<T> Where(Expression<Func<T, bool>> expression);
    public T? Add(T value);
    public Task<T?> AddAsync(T value);
    public bool Update(T value);
    public Task<bool> UpdateAsync(T value);
    public bool Remove(T value);
    public Task<bool> RemoveAsync(T value);
}
