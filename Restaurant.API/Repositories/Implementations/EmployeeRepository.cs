using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;
using Restaurant.API.Repositories.Contracts;

namespace Restaurant.API.Repositories.Implementations;

public sealed class EmployeeRepository(RestaurantDbContext context) : IEmployeeRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable<Employee> SelectAll() =>
        _context.Employees
            .Include(e => e.User)
            .Include(e => e.Role)
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<Employee> SelectByEmail(string email) =>
        _context.Employees
            .Include(e => e.User)
            .Include(e => e.Role)
            .Where(e => e.User.Email.Contains(email))
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<Employee> SelectById(Guid id) =>
        _context.Employees
            .Include(e => e.User)
            .Include(e => e.Role)
            .Where(e => e.Id == id)
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<Employee> SelectByRole(string role) =>
        _context.Employees
            .Include(e => e.User)
            .Include(e => e.Role)
            .Where(e => e.Role.Name.Contains(role))
            .AsNoTracking()
            .AsQueryable();

    public async Task<Employee?> AddAsync(User user, EmployeeRole role)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var employee = new Employee { User = user, Role = role };

            _context.Attach(employee).State = EntityState.Added;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return employee;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Attach(employee).State = EntityState.Modified;

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

    public async Task<bool> RemoveAsync(Employee employee)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Users.Where(u => u.Id == employee.User.Id).ExecuteDeleteAsync();
            await _context.Employees.Where(e => e.Id == employee.Id).ExecuteDeleteAsync();

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
