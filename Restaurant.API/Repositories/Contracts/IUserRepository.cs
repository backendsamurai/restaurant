using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface IUserRepository
{
    public IQueryable<User> SelectById(Guid id);
    public IQueryable<User> SelectByEmail(string email);
    public IQueryable<User> SelectByRole(UserRole role);
    public Task<User?> AddAsync(User user);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> RemoveAsync(User user);
}
