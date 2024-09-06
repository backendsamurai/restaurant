using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Desk;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;

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

        return desk is null ? Result.NotFound("desk not found") : Result.Success(desk);
    }

    public async Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel)
    {
        var validationResult = await _createDeskValidator.ValidateAsync(createDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskByName(createDeskModel.Name!).FirstOrDefaultAsync();

        if (desk is not null)
            return Result.Conflict("desk with this name already exists");

        var newDesk = await _deskRepository.CreateDeskAsync(createDeskModel.Name!);

        if (newDesk is not null)
        {
            await _cache.InsertAsync(newDesk.Adapt<DeskCacheModel>());
            return Result.Success(newDesk);
        }

        return Result.Error("cannot create desk");
    }

    public async Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel)
    {
        var validationResult = await _updateDeskValidator.ValidateAsync(updateDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskById(id).FirstOrDefaultAsync();

        if (desk is null)
            return Result.NotFound("desk not found");

        if (desk.Name == updateDeskModel.Name!)
            return Result.Conflict("the desk name is the same as in the database");

        desk.Name = updateDeskModel.Name!;

        var isUpdated = await _deskRepository.UpdateDeskAsync(desk);

        if (isUpdated)
        {
            await _cache.UpdateAsync(desk.Adapt<DeskCacheModel>());
            return Result.Success(desk);
        }

        return Result.Error("cannot update desk");
    }

    public async Task<Result> RemoveDeskAsync(Guid id)
    {
        var desk = await _deskRepository.SelectDeskById(id).FirstOrDefaultAsync();

        if (desk is null)
            return Result.NotFound("desk not found");

        var isRemoved = await _deskRepository.RemoveDeskAsync(desk);

        if (isRemoved)
        {
            await _cache.DeleteAsync(desk.Adapt<DeskCacheModel>());
            return Result.Success();
        }

        return Result.Error("cannot remove desk");
    }
}
