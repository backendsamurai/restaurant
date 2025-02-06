using FluentValidation;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.EmployeeRole;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class EmployeeRoleService(
    IRepository<EmployeeRole> employeeRoleRepository,
    IValidator<CreateEmployeeRoleModel> createEmployeeRoleModelValidator,
    IValidator<UpdateEmployeeRoleModel> updateEmployeeRoleModelValidator,
    IRedisCollection<EmployeeRoleCacheModel> cache
) : IEmployeeRoleService
{
    public async Task<Result<List<EmployeeRole>>> GetAllEmployeeRolesAsync() =>
        await cache.GetOrSetAsync(employeeRoleRepository.SelectAllAsync);

    public async Task<Result<EmployeeRole>> GetEmployeeRoleByIdAsync(Guid id)
    {
        var role = await cache.GetOrSetAsync(er => er.Id == id,
            async () => await employeeRoleRepository.SelectByIdAsync(id));

        return role is null ? DetailedError.NotFound("Please provide correct id") : Result.Success(role);
    }

    public async Task<Result<List<EmployeeRole>>> GetEmployeeRoleByNameAsync(string name) =>
        await cache.GetOrSetAsync(r => r.Name.Contains(name),
            async () => await employeeRoleRepository
                .WhereAsync<EmployeeRole>(er => er.Name.Contains(name)));

    public async Task<Result<EmployeeRole>> CreateEmployeeRoleAsync(CreateEmployeeRoleModel createEmployeeRoleModel)
    {
        var validationResult = await createEmployeeRoleModelValidator.ValidateAsync(createEmployeeRoleModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var roleFromDb = await employeeRoleRepository.FirstOrDefaultAsync(er => er.Name == createEmployeeRoleModel.Name!);

        if (roleFromDb is not null)
            return DetailedError.Conflict("employee role with this name already exists");

        var createdRole = await employeeRoleRepository.AddAsync(new EmployeeRole { Name = createEmployeeRoleModel.Name! });

        if (createdRole is not null)
        {
            await cache.InsertAsync(createdRole);
            return Result.Created(createdRole);
        }

        return DetailedError.CreatingProblem("Cannot Create Employee Role", "Check all provided data and try again later");
    }

    public async Task<Result<EmployeeRole>> UpdateEmployeeRoleAsync(Guid id, UpdateEmployeeRoleModel updateEmployeeRoleModel)
    {
        var validationResult = await updateEmployeeRoleModelValidator.ValidateAsync(updateEmployeeRoleModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var role = await employeeRoleRepository.SelectByIdAsync(id);

        if (role is null)
            return DetailedError.NotFound("Please provide correct id");

        if (role.Name == updateEmployeeRoleModel.Name!)
            return Result.NoContent();

        role.Name = updateEmployeeRoleModel.Name!;

        var isUpdated = await employeeRoleRepository.UpdateAsync(role);

        if (isUpdated)
        {
            await cache.UpdateAsync(role);
            return Result.Success(role);
        }

        return DetailedError.UpdatingProblem("Check all provided data and try again later");
    }

    public async Task<Result> RemoveEmployeeRoleAsync(Guid id)
    {
        var role = await employeeRoleRepository.SelectByIdAsync(id);

        if (role is null)
            return DetailedError.NotFound("Please provide correct id");

        var isRemoved = await employeeRoleRepository.RemoveAsync(role);

        if (isRemoved)
        {
            await cache.DeleteAsync(role);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Check all provided data and try again later");
    }
}
