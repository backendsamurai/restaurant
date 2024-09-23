using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;
using Restaurant.API.Repositories.Contracts;

namespace Restaurant.API.Repositories.Implementations;

public sealed class EmployeeRoleRepository(RestaurantDbContext context) : IEmployeeRoleRepository
{
    private readonly RestaurantDbContext _context = context;

    public IQueryable<EmployeeRole> SelectAll() =>
        _context.EmployeeRoles
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<EmployeeRole> SelectById(Guid id) =>
        _context.EmployeeRoles
            .Where(e => e.Id == id)
            .AsNoTracking()
            .AsQueryable();

    public IQueryable<EmployeeRole> SelectByName(string name) =>
        _context.EmployeeRoles
            .Where(e => e.Name.Contains(name))
            .AsNoTracking()
            .AsQueryable();

    public async Task<EmployeeRole?> AddAsync(string name)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var role = new EmployeeRole { Name = name };

            await _context.EmployeeRoles.AddAsync(role);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return role;
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
