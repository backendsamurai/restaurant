using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IEmployeeRoleRepository
{
    public IQueryable<EmployeeRole> SelectAll();
    public Task<EmployeeRole?> AddAsync(EmployeeRole employeeRole);
    public Task<bool> UpdateAsync(EmployeeRole employeeRole);
    public Task<bool> RemoveAsync(EmployeeRole employeeRole);
}
