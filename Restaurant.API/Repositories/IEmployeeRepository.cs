using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IEmployeeRepository
{
    public Task<List<Employee>> SelectAll();
    public Task<Employee?> SelectById(Guid id);
    public Task<Employee?> SelectByEmail(string email);
    public Task<List<Employee>> SelectByRole(string role);
    public Task<Guid> Add(Employee employee);
    public Task Update(Guid id, string name, string email, string passwordHash);
    public Task Remove(Guid id);
}
