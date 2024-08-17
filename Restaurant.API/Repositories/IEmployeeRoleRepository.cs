using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IEmployeeRoleRepository
{
    public Task<List<EmployeeRole>> SelectAll();
    public Task<Guid> Add(string name);
    public Task AddRange(string[] names);
    public Task Update(Guid id, string name);
    public Task Remove(Guid id);
}
