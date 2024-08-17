using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface ICustomerRepository
{
    public Task<Customer?> SelectById(Guid id);
    public Task<Customer?> SelectByEmail(string email);
    public Task<Guid> Add(User user);
    public Task<Customer?> Update(Guid id, string name, string email, string passwordHash);
    public Task Remove(Guid id);
}