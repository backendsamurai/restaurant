using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class CustomerRepository(RestaurantDbContext context) : ICustomerRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable SelectByEmail(string email)
    {
        return _context.Customers
            .Include(c => c.User)
            .Where(c => c.User.Email == email)
            .AsQueryable()
            .AsNoTracking();
    }

    public IQueryable SelectById(Guid id)
    {
        return _context.Customers
            .Include(c => c.User)
            .Where(c => c.Id == id)
            .AsQueryable()
            .AsNoTracking();
    }
    public async Task<Customer?> AddAsync(User user)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var customer = new Customer { User = user };
            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return customer;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public Task<Customer?> UpdateAsync(Guid id, string name, string email, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
