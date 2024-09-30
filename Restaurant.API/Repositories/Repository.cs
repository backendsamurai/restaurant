using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories;

public class Repository<T>(RestaurantDbContext context) : IRepository<T> where T : class
{
    private readonly RestaurantDbContext _context = context;

    public List<T> SelectAll() => [.. _context.Set<T>()];

    public async Task<List<T>> SelectAllAsync() =>
        await _context.Set<T>().ToListAsync();

    public T? SelectById(Guid id) => _context.Set<T>().Find(id);

    public async Task<T?> SelectByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);

    public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression) =>
        _context.Set<T>().Select(expression);

    public IQueryable<T> Where(Expression<Func<T, bool>> expression) =>
       _context.Set<T>().Where(expression);

    public T? Add(T value)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Entry(value).State = EntityState.Added;
            _context.SaveChanges();
            transaction.Commit();

            return value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return null;
        }
    }

    public async Task<T?> AddAsync(T value)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Entry(value).State = EntityState.Added;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return value;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public bool Update(T value)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Entry(value).State = EntityState.Modified;
            _context.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<bool> UpdateAsync(T value)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public bool Remove(T value)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Entry(value).State = EntityState.Deleted;
            _context.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<bool> RemoveAsync(T value)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Entry(value).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
