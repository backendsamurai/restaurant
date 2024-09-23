using Restaurant.API.Entities;

namespace Restaurant.API.Repositories.Contracts;

public interface IDeskRepository
{
    public IQueryable<Desk> SelectAllDesks();
    public IQueryable<Desk> SelectDeskById(Guid id);
    public IQueryable<Desk> SelectDeskByName(string name);
    public Task<Desk?> CreateDeskAsync(string name);
    public Task<bool> UpdateDeskAsync(Desk desk);
    public Task<bool> RemoveDeskAsync(Desk desk);
}
