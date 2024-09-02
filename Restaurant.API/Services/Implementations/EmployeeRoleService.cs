using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;
using Restaurant.API.Models.EmployeeRole;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Services.Implementations;

public sealed class EmployeeRoleService(
    IEmployeeRoleRepository employeeRoleRepository,
    IValidator<CreateEmployeeRoleModel> createEmployeeRoleModelValidator,
    IValidator<UpdateEmployeeRoleModel> updateEmployeeRoleModelValidator
) : IEmployeeRoleService
{
    private readonly IEmployeeRoleRepository _employeeRoleRepository = employeeRoleRepository;
    private readonly IValidator<CreateEmployeeRoleModel> _createEmployeeRoleModelValidator = createEmployeeRoleModelValidator;
    private readonly IValidator<UpdateEmployeeRoleModel> _updateEmployeeRoleModelValidator = updateEmployeeRoleModelValidator;

    public async Task<Result<List<EmployeeRole>>> GetAllEmployeeRolesAsync() =>
        Result.Success(await _employeeRoleRepository.SelectAll().ToListAsync());

    public async Task<Result<EmployeeRole>> GetEmployeeRoleByIdAsync(Guid id)
    {
        var role = await _employeeRoleRepository
            .SelectById(id)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        return role is null ? Result.NotFound("employee role not found") : Result.Success(role);
    }

    public async Task<Result<List<EmployeeRole>>> GetEmployeeRoleByNameAsync(string name)
    {
        var roles = await _employeeRoleRepository
            .SelectByName(name)
            .ProjectToType<EmployeeRole>()
            .ToListAsync();

        return Result.Success(roles);
    }

    public async Task<Result<EmployeeRole>> CreateEmployeeRoleAsync(CreateEmployeeRoleModel createEmployeeRoleModel)
    {
        var validationResult = await _createEmployeeRoleModelValidator.ValidateAsync(createEmployeeRoleModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var roleFromDb = await _employeeRoleRepository
            .SelectByName(createEmployeeRoleModel.Name!)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (roleFromDb is not null)
            return Result.Conflict("employee role with this name already exists");

        var createdRole = await _employeeRoleRepository.AddAsync(createEmployeeRoleModel.Name!);

        return createdRole is null ? Result.Error("cannot create employee role") : Result.Success(createdRole);
    }

    public async Task<Result<EmployeeRole>> UpdateEmployeeRoleAsync(Guid id, UpdateEmployeeRoleModel updateEmployeeRoleModel)
    {
        var validationResult = await _updateEmployeeRoleModelValidator.ValidateAsync(updateEmployeeRoleModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var role = await _employeeRoleRepository
            .SelectById(id)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (role is null)
            return Result.NotFound("employee role not found");

        if (role.Name == updateEmployeeRoleModel.Name!)
            return Result.Conflict("the employee role name is the same as in the database");

        role.Name = updateEmployeeRoleModel.Name!;

        var isUpdated = await _employeeRoleRepository.UpdateAsync(role);

        return isUpdated ? Result.Success(role) : Result.Error("cannot update employee role");
    }

    public async Task<Result> RemoveEmployeeRoleAsync(Guid id)
    {
        var role = await _employeeRoleRepository
            .SelectById(id)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (role is null)
            return Result.NotFound("employee role not found");

        var isRemoved = await _employeeRoleRepository.RemoveAsync(role);

        return isRemoved ? Result.Success() : Result.Error("cannot remove employee role");
    }
}
