using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IEmployeeRoleRepository
{
    public IQueryable<EmployeeRole> SelectAll();
    public IQueryable<EmployeeRole> SelectById(Guid id);
    public IQueryable<EmployeeRole> SelectByName(string name);
    public Task<EmployeeRole?> AddAsync(string name);
    public Task<bool> UpdateAsync(EmployeeRole employeeRole);
    public Task<bool> RemoveAsync(EmployeeRole employeeRole);
}
