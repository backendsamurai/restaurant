using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Employee;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class EmployeeService(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleRepository employeeRoleRepository,
    IPasswordHasherService passwordHasher,
    IValidator<CreateEmployeeModel> createEmployeeModelValidator,
    IValidator<UpdateEmployeeModel> updateEmployeeModelValidator,
    IRedisCollection<EmployeeCacheModel> cache
) : IEmployeeService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository = employeeRoleRepository;
    private readonly IPasswordHasherService _passwordHasher = passwordHasher;
    private readonly IValidator<CreateEmployeeModel> _createEmployeeModelValidator = createEmployeeModelValidator;
    private readonly IValidator<UpdateEmployeeModel> _updateEmployeeModelValidator = updateEmployeeModelValidator;
    private readonly IRedisCollection<EmployeeCacheModel> _cache = cache;

    public async Task<Result<List<EmployeeResponse>>> GetAllEmployeesAsync() =>
        await _cache.GetOrSetAsync(
            async () => await _employeeRepository.SelectAll().ProjectToType<EmployeeResponse>().ToListAsync());

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByEmailAsync(string email) =>
         await _cache.GetOrSetAsync(e => e.UserEmail.StartsWith(email),
            async () => await _employeeRepository.SelectByEmail(email).ProjectToType<EmployeeResponse>().ToListAsync());

    public async Task<Result<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _cache.GetOrSetAsync(e => e.EmployeeId == id,
                async () => await _employeeRepository
                    .SelectById(id)
                    .ProjectToType<EmployeeResponse>()
                    .FirstOrDefaultAsync());

        return employee is null
            ? Result.NotFound(
                code: "EMP-440-001",
                type: "entity_not_found",
                message: "Employee not found",
                detail: "Please provide correct id"
            ) : Result.Success(employee);
    }

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByRoleAsync(string role) =>
        await _cache.GetOrSetAsync(e => e.EmployeeRole.StartsWith(role),
            async () => await _employeeRepository.SelectByRole(role).ProjectToType<EmployeeResponse>().ToListAsync());

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel)
    {
        var validationResult = await _createEmployeeModelValidator.ValidateAsync(createEmployeeModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "EMP-440-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var userFromDb = await _userRepository
            .SelectByEmail(createEmployeeModel.Email!)
            .ProjectToType<User>()
            .FirstOrDefaultAsync();

        if (userFromDb is not null)
            return Result.Conflict(
                code: "EMP-440-003",
                type: "entity_already_exists",
                message: "Employee with this email already exists",
                detail: "Please check provided email or provide another email address"
            );

        var employeeRole = await _employeeRoleRepository
            .SelectByName(createEmployeeModel.Role!)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (employeeRole is null)
            return Result.NotFound(
                code: "EMP-440-001",
                type: "entity_not_found",
                message: "Employee Role not found",
                detail: "Please provide correct role name"
            );

        var passwordHash = _passwordHasher.Hash(createEmployeeModel.Password!);

        var newUser = await _userRepository.AddAsync(Tuple.Create(createEmployeeModel, passwordHash).Adapt<User>());

        if (newUser is not null)
        {
            var newEmployee = await _employeeRepository.AddAsync(newUser, employeeRole);

            if (newEmployee is not null)
            {
                await _cache.InsertAsync(newEmployee);
                return Result.Created(newEmployee.Adapt<EmployeeResponse>());
            }
        }

        return Result.Error(
            code: "CSR-554-001",
            type: "error_while_creation_employee",
            message: "Cannot create employee",
            detail: "Unexpected error"
        );
    }

    public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
    {
        bool isModified = false;

        var employee = await _employeeRepository
            .SelectById(id)
            .ProjectToType<Employee>()
            .FirstOrDefaultAsync();

        if (employee is null)
            return Result.NotFound(
                code: "EMP-440-001",
                type: "entity_not_found",
                message: "Employee not found",
                detail: "Please provide correct id"
            );

        if (updateEmployeeModel.Name is not null && updateEmployeeModel.Name != employee.User.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("name"));

            if (!validationResult.IsValid)
                return Result.Invalid(
                   code: "EMP-440-002",
                   type: "invalid_model",
                   message: "One of field are not valid",
                   detail: "Check all fields and try again"
               );

            employee.User.Name = updateEmployeeModel.Name;
            isModified = true;
        }

        if (updateEmployeeModel.Email is not null && updateEmployeeModel.Email != employee.User.Email)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("email"));

            if (!validationResult.IsValid)
                return Result.Invalid(
                   code: "EMP-440-002",
                   type: "invalid_model",
                   message: "One of field are not valid",
                   detail: "Check all fields and try again"
               );

            employee.User.Email = updateEmployeeModel.Email;
            isModified = true;
        }

        if (updateEmployeeModel.Password is not null && !_passwordHasher.Verify(updateEmployeeModel.Password, employee.User.PasswordHash))
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("password"));

            if (!validationResult.IsValid)
                return Result.Invalid(
                   code: "EMP-440-002",
                   type: "invalid_model",
                   message: "One of field are not valid",
                   detail: "Check all fields and try again"
               );

            employee.User.PasswordHash = _passwordHasher.Hash(updateEmployeeModel.Password);
            isModified = true;
        }

        if (updateEmployeeModel.Role is not null && updateEmployeeModel.Role != employee.Role.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("role"));

            if (!validationResult.IsValid)
                return Result.Invalid(
                   code: "EMP-440-002",
                   type: "invalid_model",
                   message: "One of field are not valid",
                   detail: "Check all fields and try again"
               );

            var employeeRole = await _employeeRoleRepository
                .SelectByName(updateEmployeeModel.Role)
                .ProjectToType<EmployeeRole>()
                .FirstOrDefaultAsync();

            if (employeeRole is null)
                return Result.NotFound(
                    code: "EMP-440-001",
                    type: "entity_not_found",
                    message: "Employee Role not found",
                    detail: "Please provide correct role name"
                );

            employee.Role = employeeRole;
            isModified = true;
        }

        if (isModified)
        {
            var isUpdated = await _employeeRepository.UpdateAsync(employee);

            if (isUpdated)
            {
                await _cache.UpdateAsync(employee);
                return Result.Success(employee.Adapt<EmployeeResponse>());
            }

            return Result.Error(
                code: "CSR-554-002",
                type: "error_while_updating_employee",
                message: "Cannot update employee",
                detail: "Unexpected error"
            );
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository
            .SelectById(id)
            .ProjectToType<Employee>()
            .FirstOrDefaultAsync();

        if (employee is null)
            return Result.NotFound(
                code: "EMP-440-001",
                type: "entity_not_found",
                message: "Employee not found",
                detail: "Please provide correct id"
            );

        var isRemoved = await _employeeRepository.RemoveAsync(employee);

        if (isRemoved)
        {
            await _cache.DeleteAsync(employee);
            return Result.NoContent();
        }

        return Result.Error(
            code: "CSR-554-003",
            type: "error_while_removing_employee",
            message: "Cannot remove employee",
            detail: "Unexpected error"
        );
    }
}
