using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IUserRepository
{
    public Task<User?> SelectByIdAsync(Guid id);
    public Task<User?> SelectByEmailAsync(string email);
    public Task<User?> SelectByRoleAsync(UserRole role);
    public Task<User?> AddAsync(User user);
    public Task UpdateAsync(Guid id, string email, string name, string passwordHash);
    public Task RemoveAsync(Guid id);
}
