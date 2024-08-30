using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class EmployeeRoleRepository(RestaurantDbContext context) : IEmployeeRoleRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable<EmployeeRole> SelectAll() =>
        _context.EmployeeRoles
            .AsNoTracking()
            .AsQueryable();

    public async Task<EmployeeRole?> AddAsync(EmployeeRole employeeRole)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.EmployeeRoles.AddAsync(employeeRole);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return employeeRole;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> UpdateAsync(EmployeeRole employeeRole)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.EmployeeRoles.Update(employeeRole);

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

    public async Task<bool> RemoveAsync(EmployeeRole employeeRole)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.EmployeeRoles.Remove(employeeRole);

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
