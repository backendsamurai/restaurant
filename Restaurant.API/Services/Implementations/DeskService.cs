using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Services.Implementations;

public sealed class DeskService(
    IDeskRepository repository,
    IValidator<CreateDeskModel> createDeskValidator,
    IValidator<UpdateDeskModel> updateDeskValidator
) : IDeskService
{
    private readonly IDeskRepository _deskRepository = repository;
    private readonly IValidator<CreateDeskModel> _createDeskValidator = createDeskValidator;
    private readonly IValidator<UpdateDeskModel> _updateDeskValidator = updateDeskValidator;

    public async Task<Result<List<Desk>>> GetAllDesksAsync() =>
        Result.Success(await _deskRepository.SelectAllDesksAsync());


    public async Task<Result<Desk>> GetDeskByIdAsync(Guid id)
    {
        var desk = await _deskRepository.SelectDeskByIdAsync(id);

        if (desk is null)
            return Result.NotFound("desk not found");

        return Result.Success(desk);
    }

    public async Task<Result<Desk>> GetDeskByNameAsync(string name)
    {
        var desk = await _deskRepository.SelectDeskByNameAsync(name);

        if (desk is null)
            return Result.NotFound("desk not found");

        return Result.Success(desk);
    }

    public async Task<Result<Desk>> CreateDeskAsync(CreateDeskModel createDeskModel)
    {
        var validationResult = await _createDeskValidator.ValidateAsync(createDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskByNameAsync(createDeskModel.Name!);

        if (desk is not null)
            return Result.Conflict("desk with this name already exists");

        var newDesk = await _deskRepository.CreateDeskAsync(createDeskModel.Name!);

        if (newDesk is null)
            return Result.Error("cannot create desk");

        return Result.Success(newDesk);
    }

    public async Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskModel updateDeskModel)
    {
        var validationResult = await _updateDeskValidator.ValidateAsync(updateDeskModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskByIdAsync(id);

        if (desk is null)
            return Result.NotFound("desk not found");

        if (desk.Name == updateDeskModel.Name!)
            return Result.Conflict("the desk name is the same as in the database");

        desk.Name = updateDeskModel.Name!;

        var isUpdated = await _deskRepository.UpdateDeskAsync(desk);

        return isUpdated ? Result.Success(desk) : Result.Error("cannot update desk");
    }

    public async Task<Result> RemoveDeskAsync(Guid id)
    {
        var desk = await _deskRepository.SelectDeskByIdAsync(id);

        if (desk is null)
            return Result.NotFound("desk not found");

        var isRemoved = await _deskRepository.RemoveDeskAsync(desk);

        return isRemoved ? Result.Success() : Result.Error("cannot remove desk");
    }
}
