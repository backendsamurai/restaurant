using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IUserRepository
{
    public Task<User?> SelectById(Guid id);
    public Task<User?> SelectByEmail(string email);
    public Task<User?> SelectByRole(UserRole role);
    public Task<Guid> Add(User user);
    public Task Update(Guid id, string email, string name, string passwordHash);
    public Task Remove(Guid id);
}
