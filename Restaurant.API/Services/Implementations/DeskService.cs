using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Desk;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class DeskService(
    IRepository<Desk> repository,
    IValidator<CreateDeskModel> createDeskValidator,
    IValidator<UpdateDeskModel> updateDeskValidator,
    IRedisCollection<DeskCacheModel> cache,
    ILogger<DeskService> logger
) : IDeskService
{

    public async Task<Result<List<Desk>>> GetAllDesksAsync() =>
        await cache.GetOrSetAsync(repository.SelectAllAsync);

    public async Task<Result<Desk>> GetDeskByIdAsync(Guid id)
    {
        var desk = await cache.GetOrSetAsync(d => d.Id == id, async () => await repository.SelectByIdAsync(id));

        return desk is null ? DetailedError.NotFound("Please provide correct id") : Result.Success(desk);
    }

    public async Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel)
    {
        var validationResult = await createDeskValidator.ValidateAsync(createDeskModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var desk = await repository.Where(d => d.Name == createDeskModel.Name!).FirstOrDefaultAsync();

        if (desk is not null)
            return DetailedError.Conflict("Desk is already exists", "Please verify parameter 'name'");

        var newDesk = await repository.AddAsync(new Desk { Name = createDeskModel.Name! });

        if (newDesk is not null)
        {
            try
            {
                await cache.InsertAsync(newDesk);
            }
            catch (Exception)
            {
                logger.LogWarning("Cannot write data into cache. Cache unavailable!");
            }
            return Result.Created(newDesk);
        }

        return DetailedError.CreatingProblem("Cannot create desk", "Unexpected error");
    }

    public async Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel)
    {
        var validationResult = await updateDeskValidator.ValidateAsync(updateDeskModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var desk = await repository.SelectByIdAsync(id);

        if (desk is null)
            return DetailedError.NotFound("Please provide correct id");

        if (desk.Name == updateDeskModel.Name!)
            return Result.NoContent();

        desk.Name = updateDeskModel.Name!;

        var isUpdated = await repository.UpdateAsync(desk);

        if (isUpdated)
        {
            await cache.UpdateAsync(desk);
            return Result.Success(desk);
        }

        return DetailedError.UpdatingProblem("Error while updating desk", "Check all provided data and try again later");
    }

    public async Task<Result> RemoveDeskAsync(Guid id)
    {
        var desk = await repository.SelectByIdAsync(id);

        if (desk is null)
            return DetailedError.NotFound("Please provide correct id");

        var isRemoved = await repository.RemoveAsync(desk);

        if (isRemoved)
        {
            await cache.DeleteAsync(desk);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Cannot remove desk", "Unexpected error");
    }
}
