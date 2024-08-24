using Restaurant.API.Entities;

namespace Restaurant.API.Repositories;

public interface IDeskRepository
{
    public Task<List<Desk>> SelectAllDesksAsync();
    public Task<Desk?> SelectDeskByIdAsync(Guid id);
    public Task<Desk?> SelectDeskByNameAsync(string name);
    public Task<Desk?> CreateDeskAsync(string name);
    public Task<bool> UpdateDeskAsync(Desk desk);
    public Task<bool> RemoveDeskAsync(Desk desk);
}
