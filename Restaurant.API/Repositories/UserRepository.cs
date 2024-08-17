using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public sealed class UserRepository : IUserRepository
{
    public Task<User?> SelectById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> SelectByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User?> SelectByRole(UserRole role)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Add(User user)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, string email, string name, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}