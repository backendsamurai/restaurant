using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class EmployeeRoleRepository : IEmployeeRoleRepository
{
    public Task<List<EmployeeRole>> SelectAll()
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Add(string name)
    {
        throw new NotImplementedException();
    }

    public Task AddRange(string[] names)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, string name)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}
