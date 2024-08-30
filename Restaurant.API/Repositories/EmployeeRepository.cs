using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

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
            .Where(e => e.User.Email == email)
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
            .Where(e => e.Role.Name == role)
            .AsNoTracking()
            .AsQueryable();

    public async Task<Employee?> AddAsync(Employee employee)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Employees.AddAsync(employee);

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
            _context.Employees.Update(employee);

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
            _context.Employees.Remove(employee);

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
