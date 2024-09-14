using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Contracts;

public interface IDeskService
{
    public Task<Result<List<Desk>>> GetAllDesksAsync();
    public Task<Result<Desk>> GetDeskByIdAsync(Guid id);
    public Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel);
    public Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel);
    public Task<Result> RemoveDeskAsync(Guid id);
}
