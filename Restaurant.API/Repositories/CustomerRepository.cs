using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    public Task<Customer?> SelectByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> SelectById(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<Guid> Add(User user)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> Update(Guid id, string name, string email, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}
