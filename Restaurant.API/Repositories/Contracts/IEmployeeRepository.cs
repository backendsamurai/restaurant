using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface IEmployeeRepository
{
    public IQueryable<Employee> SelectAll();
    public IQueryable<Employee> SelectById(Guid id);
    public IQueryable<Employee> SelectByEmail(string email);
    public IQueryable<Employee> SelectByRole(string role);
    public Task<Employee?> AddAsync(User user, EmployeeRole role);
    public Task<bool> UpdateAsync(Employee employee);
    public Task<bool> RemoveAsync(Employee employee);
}
