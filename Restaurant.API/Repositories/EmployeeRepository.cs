using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    public Task<List<Employee>> SelectAll()
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> SelectByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> SelectById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Employee>> SelectByRole(string role)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Add(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, string name, string email, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}
