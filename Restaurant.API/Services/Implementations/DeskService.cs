using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Desk;
using Restaurant.API.Repositories.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class DeskService(
    IDeskRepository repository,
    IValidator<CreateDeskModel> createDeskValidator,
    IValidator<UpdateDeskModel> updateDeskValidator,
    IRedisCollection<DeskCacheModel> cache
) : IDeskService
{
    private readonly IDeskRepository _deskRepository = repository;
    private readonly IValidator<CreateDeskModel> _createDeskValidator = createDeskValidator;
    private readonly IValidator<UpdateDeskModel> _updateDeskValidator = updateDeskValidator;
    private readonly IRedisCollection<DeskCacheModel> _cache = cache;

    public async Task<Result<List<Desk>>> GetAllDesksAsync() =>
        await _cache.GetOrSetAsync(async () => await _deskRepository.SelectAllDesks().ToListAsync());

    public async Task<Result<Desk>> GetDeskByIdAsync(Guid id)
    {
        var desk = await _cache.GetOrSetAsync(d => d.Id == id, async () =>
            await _deskRepository.SelectDeskById(id).FirstOrDefaultAsync());

        return desk is null
            ? Result.NotFound(
                code: "DSK-000-001",
                type: "entity_not_found",
                message: "Desk not found",
                detail: "Please provide correct id"
            ) : Result.Success(desk);
    }

    public async Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel)
    {
        var validationResult = await _createDeskValidator.ValidateAsync(createDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "DSK-000-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var desk = await _deskRepository.SelectDeskByName(createDeskModel.Name!).FirstOrDefaultAsync();

        if (desk is not null)
            return Result.Conflict(
                code: "DSK-000-003",
                type: "conflict_entities",
                message: "Desk is already exists",
                detail: "Please verify parameter 'name'"
            );

        var newDesk = await _deskRepository.CreateDeskAsync(createDeskModel.Name!);

        if (newDesk is not null)
        {
            await _cache.InsertAsync(newDesk);
            return Result.Created(newDesk);
        }

        return Result.Error(
            code: "DSK-100-001",
            type: "error_create_desk",
            message: "Cannot create desk",
            detail: "Unexpected error"
        );
    }

    public async Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel)
    {
        var validationResult = await _updateDeskValidator.ValidateAsync(updateDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "DSK-000-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var desk = await _deskRepository.SelectDeskById(id).FirstOrDefaultAsync();

        if (desk is null)
            return Result.NotFound(
                code: "DSK-000-001",
                type: "entity_not_found",
                message: "Desk not found",
                detail: "Please provide correct id"
            );

        if (desk.Name == updateDeskModel.Name!)
            return Result.NoContent();

        desk.Name = updateDeskModel.Name!;

        var isUpdated = await _deskRepository.UpdateDeskAsync(desk);

        if (isUpdated)
        {
            await _cache.UpdateAsync(desk);
            return Result.Success(desk);
        }

        return Result.Error(
            code: "DSK-100-002",
            type: "entity_update_error",
            message: "Error while updating desk",
            detail: "Check all provided data and try again later"
        );
    }

    public async Task<Result> RemoveDeskAsync(Guid id)
    {
        var desk = await _deskRepository.SelectDeskById(id).FirstOrDefaultAsync();

        if (desk is null)
            return Result.NotFound(
                code: "DSK-000-001",
                type: "entity_not_found",
                message: "Desk not found",
                detail: "Please provide correct id"
            );

        var isRemoved = await _deskRepository.RemoveDeskAsync(desk);

        if (isRemoved)
        {
            await _cache.DeleteAsync(desk);
            return Result.NoContent();
        }

        return Result.Error(
            code: "DSK-100-003",
            type: "error_while_remove_desk",
            message: "Cannot remove desk",
            detail: "Unexpected error"
        );
    }
}
