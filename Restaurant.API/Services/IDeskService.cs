using Ardalis.Result;
using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;

namespace Restaurant.API.Services;

public interface IDeskService
{
    public Task<Result<List<Desk>>> GetAllDesksAsync();
    public Task<Result<Desk>> GetDeskByIdAsync(Guid id);
    public Task<Result<Desk>> GetDeskByNameAsync(string name);
    public Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel);
    public Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel);
    public Task<Result> RemoveDeskAsync(Guid id);
}
