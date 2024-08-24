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

    public async Task<bool> UpdateAsync(User user)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> RemoveAsync(Customer customer)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Users.Where(u => u.Id == customer.User.Id).ExecuteDeleteAsync();
            await _context.Customers.Where(c => c.Id == customer.Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
