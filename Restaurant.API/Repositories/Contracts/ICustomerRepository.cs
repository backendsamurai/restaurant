using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface ICustomerRepository
{
    public IQueryable<Customer> SelectById(Guid id);
    public IQueryable<Customer> SelectByEmail(string email);
    public Task<Customer?> AddAsync(User user);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> RemoveAsync(Customer customer);
}