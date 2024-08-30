using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class UserRepository(RestaurantDbContext context) : IUserRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable<User> SelectById(Guid id) =>
        _context.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<User> SelectByEmail(string email) =>
        _context.Users
            .Where(u => u.Email == email)
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<User> SelectByRole(UserRole role) =>
        _context.Users
            .Where(u => u.Role == role)
            .AsNoTracking()
            .AsQueryable();

    public async Task<User?> AddAsync(User user)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return user;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Users.Update(user);

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

    public async Task<bool> RemoveAsync(User user)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Users.Remove(user);

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