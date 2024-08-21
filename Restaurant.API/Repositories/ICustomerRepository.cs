using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface ICustomerRepository
{
    public IQueryable SelectById(Guid id);
    public IQueryable SelectByEmail(string email);
    public Task<Customer?> AddAsync(User user);
    public Task<Customer?> UpdateAsync(Guid id, string name, string email, string passwordHash);
    public Task RemoveAsync(Guid id);
}