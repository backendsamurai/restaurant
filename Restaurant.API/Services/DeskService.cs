using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;
using Restaurant.API.Repositories;

namespace Restaurant.API.Services;

public sealed class DeskService(
    IDeskRepository repository,
    IValidator<CreateDeskRequest> createDeskValidator,
    IValidator<UpdateDeskRequest> updateDeskValidator
) : IDeskService
{
    private readonly IDeskRepository _deskRepository = repository;
    private readonly IValidator<CreateDeskRequest> _createDeskValidator = createDeskValidator;
    private readonly IValidator<UpdateDeskRequest> _updateDeskValidator = updateDeskValidator;

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

    public async Task<Result<Desk>> CreateDeskAsync(CreateDeskRequest createDeskRequest)
    {
        var validationResult = await _createDeskValidator.ValidateAsync(createDeskRequest);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskByNameAsync(createDeskRequest.Name!);

        if (desk is not null)
            return Result.Conflict("desk with this name already exists");

        var newDesk = await _deskRepository.CreateDeskAsync(createDeskRequest.Name!);

        if (newDesk is null)
            return Result.Error("cannot create desk");

        return Result.Success(newDesk);
    }

    public async Task<Result<Desk>> UpdateDeskAsync(Guid id, UpdateDeskRequest updateDeskRequest)
    {
        var validationResult = await _updateDeskValidator.ValidateAsync(updateDeskRequest);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var desk = await _deskRepository.SelectDeskByIdAsync(id);

        if (desk is null)
            return Result.NotFound("desk not found");

        desk.Name = updateDeskRequest.Name!;

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
