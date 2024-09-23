using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;
using Restaurant.API.Repositories.Contracts;

namespace Restaurant.API.Repositories.Implementations;

public sealed class CustomerRepository(RestaurantDbContext context) : ICustomerRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable<Customer> SelectByEmail(string email) =>
        _context.Customers
            .Include(c => c.User)
            .Where(c => c.User.Email.Contains(email))
            .AsQueryable()
            .AsNoTracking();


    public IQueryable<Customer> SelectById(Guid id) =>
        _context.Customers
            .Include(c => c.User)
            .Where(c => c.Id == id)
            .AsQueryable()
            .AsNoTracking();

    public async Task<Customer?> AddAsync(User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var customer = new Customer { User = user };
            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return customer;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

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

    public async Task<bool> RemoveAsync(Customer customer)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Users.Where(u => u.Id == customer.User.Id).ExecuteDeleteAsync();
            await _context.Customers.Where(c => c.Id == customer.Id).ExecuteDeleteAsync();

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
