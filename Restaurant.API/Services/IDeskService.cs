using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;

namespace Restaurant.API.Services;

public interface IDeskService
{
    public Task<Result<List<Desk>>> GetAllDesksAsync();
    public Task<Result<Desk>> GetDeskByIdAsync(Guid id);
    public Task<Result<Desk>> GetDeskByNameAsync(string name);
    public Task<Result<Desk>> CreateDeskAsync(CreateDeskRequest createDeskRequest);
    public Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskRequest updateDeskRequest);
    public Task<Result> RemoveDeskAsync(Guid id);
}
