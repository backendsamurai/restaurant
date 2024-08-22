using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface ICustomerRepository
{
    public IQueryable SelectById(Guid id);
    public IQueryable SelectByEmail(string email);
    public Task<Customer?> AddAsync(User user);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> RemoveAsync(Customer customer);
}