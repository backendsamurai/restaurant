using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.EmployeeRole;
using Restaurant.API.Repositories.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class EmployeeRoleService(
    IEmployeeRoleRepository employeeRoleRepository,
    IValidator<CreateEmployeeRoleModel> createEmployeeRoleModelValidator,
    IValidator<UpdateEmployeeRoleModel> updateEmployeeRoleModelValidator,
    IRedisCollection<EmployeeRoleCacheModel> cache
) : IEmployeeRoleService
{
    private readonly IEmployeeRoleRepository _employeeRoleRepository = employeeRoleRepository;
    private readonly IValidator<CreateEmployeeRoleModel> _createEmployeeRoleModelValidator = createEmployeeRoleModelValidator;
    private readonly IValidator<UpdateEmployeeRoleModel> _updateEmployeeRoleModelValidator = updateEmployeeRoleModelValidator;
    private readonly IRedisCollection<EmployeeRoleCacheModel> _cache = cache;

    public async Task<Result<List<EmployeeRole>>> GetAllEmployeeRolesAsync() =>
        await _cache.GetOrSetAsync(async () => await _employeeRoleRepository.SelectAll().ToListAsync());

    public async Task<Result<EmployeeRole>> GetEmployeeRoleByIdAsync(Guid id)
    {
        var role = await _cache.GetOrSetAsync(er => er.Id == id,
            async () => await _employeeRoleRepository.SelectById(id).ProjectToType<EmployeeRole>().FirstOrDefaultAsync());

        return role is null
            ? Result.NotFound(
                code: "EMR-000-001",
                type: "entity_not_found",
                message: "Employee Role not found",
                detail: "Please provide correct id"
            ) : Result.Success(role);
    }

    public async Task<Result<List<EmployeeRole>>> GetEmployeeRoleByNameAsync(string name)
    {
        var roles = await _cache.GetOrSetAsync(r => r.Name.Contains(name),
            async () => await _employeeRoleRepository.SelectByName(name).ProjectToType<EmployeeRole>().ToListAsync());

        return Result.Success(roles);
    }

    public async Task<Result<EmployeeRole>> CreateEmployeeRoleAsync(CreateEmployeeRoleModel createEmployeeRoleModel)
    {
        var validationResult = await _createEmployeeRoleModelValidator.ValidateAsync(createEmployeeRoleModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "EMR-000-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var roleFromDb = await _employeeRoleRepository
            .SelectByName(createEmployeeRoleModel.Name!)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (roleFromDb is not null)
            return Result.Conflict(
                code: "EMR-000-003",
                type: "entity_already_exists",
                message: "Employee Role already exists",
                detail: "employee role with this name already exists"
            );

        var createdRole = await _employeeRoleRepository.AddAsync(createEmployeeRoleModel.Name!);

        if (createdRole is not null)
        {
            await _cache.InsertAsync(createdRole);
            return Result.Created(createdRole);
        }

        return Result.Error(
            code: "EMR-100-001",
            type: "error_while_create_entity",
            message: "Cannot Create Employee Role",
            detail: "Check all provided data and try again later"
        );
    }

    public async Task<Result<EmployeeRole>> UpdateEmployeeRoleAsync(Guid id, UpdateEmployeeRoleModel updateEmployeeRoleModel)
    {
        var validationResult = await _updateEmployeeRoleModelValidator.ValidateAsync(updateEmployeeRoleModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "EMR-000-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var role = await _employeeRoleRepository
            .SelectById(id)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (role is null)
            return Result.NotFound(
                code: "EMR-000-001",
                type: "entity_not_found",
                message: "Employee Role not found",
                detail: "Please provide correct id"
            );

        if (role.Name == updateEmployeeRoleModel.Name!)
            return Result.NoContent();

        role.Name = updateEmployeeRoleModel.Name!;

        var isUpdated = await _employeeRoleRepository.UpdateAsync(role);

        if (isUpdated)
        {
            await _cache.UpdateAsync(role);
            return Result.Success(role);
        }

        return Result.Error(
            code: "EMR-100-002",
            type: "error_while_updating_entity",
            message: "Cannot Updated Employee Role",
            detail: "Check all provided data and try again later"
        );
    }

    public async Task<Result> RemoveEmployeeRoleAsync(Guid id)
    {
        var role = await _employeeRoleRepository
            .SelectById(id)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (role is null)
            return Result.NotFound(
                code: "EMR-000-001",
                type: "entity_not_found",
                message: "Employee Role not found",
                detail: "Please provide correct id"
            );

        var isRemoved = await _employeeRoleRepository.RemoveAsync(role);

        if (isRemoved)
        {
            await _cache.DeleteAsync(role);
            return Result.NoContent();
        }

        return Result.Error(
            code: "EMR-100-003",
            type: "error_while_removing_entity",
            message: "Cannot Remove Employee Role",
            detail: "Check all provided data and try again later"
        );
    }
}
