using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class UserRepository(RestaurantDbContext context) : IUserRepository
{
    private readonly RestaurantDbContext _context = context;

    public Task<User?> SelectByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> SelectByEmailAsync(string email) =>
         await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> SelectByRoleAsync(UserRole role)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> AddAsync(User user)
    {
        var transaction = _context.Database.BeginTransaction();

        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return user;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public Task UpdateAsync(Guid id, string email, string name, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}