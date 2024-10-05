using FluentValidation;
using Mapster;
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
    IRepository<User> userRepository,
    IRepository<Employee> employeeRepository,
    IRepository<EmployeeRole> employeeRoleRepository,
    IPasswordHasherService passwordHasher,
    IValidator<CreateEmployeeModel> createEmployeeModelValidator,
    IValidator<UpdateEmployeeModel> updateEmployeeModelValidator,
    IRedisCollection<EmployeeCacheModel> cache
) : IEmployeeService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<Employee> _employeeRepository = employeeRepository;
    private readonly IRepository<EmployeeRole> _employeeRoleRepository = employeeRoleRepository;
    private readonly IPasswordHasherService _passwordHasher = passwordHasher;
    private readonly IValidator<CreateEmployeeModel> _createEmployeeModelValidator = createEmployeeModelValidator;
    private readonly IValidator<UpdateEmployeeModel> _updateEmployeeModelValidator = updateEmployeeModelValidator;
    private readonly IRedisCollection<EmployeeCacheModel> _cache = cache;

    public async Task<Result<List<EmployeeResponse>>> GetAllEmployeesAsync() =>
        await _cache.GetOrSetAsync(_employeeRepository.SelectAllAsync<EmployeeResponse>);

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByEmailAsync(string email) =>
         await _cache.GetOrSetAsync(e => e.UserEmail.StartsWith(email),
            async () => await _employeeRepository.WhereAsync<EmployeeResponse>(e => e.User.Email.Contains(email)));

    public async Task<Result<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _cache.GetOrSetAsync(e => e.EmployeeId == id,
                async () => await _employeeRepository.WhereFirstAsync<EmployeeResponse>(e => e.Id == id));

        return employee is null
            ? Result.NotFound(
                code: "EMP-000-001",
                type: "entity_not_found",
                message: "Employee not found",
                detail: "Please provide correct id"
            ) : Result.Success(employee);
    }

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByRoleAsync(string role) =>
        await _cache.GetOrSetAsync(e => e.EmployeeRole.StartsWith(role),
            async () => await _employeeRepository.WhereAsync<EmployeeResponse>(e => e.Role.Name.StartsWith(role)));

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel)
    {
        var validationResult = await _createEmployeeModelValidator.ValidateAsync(createEmployeeModel);

        if (!validationResult.IsValid)
            return Result.Invalid(
                code: "EMP-000-002",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );

        var userFromDb = await _userRepository.FirstOrDefaultAsync(u => u.Email == createEmployeeModel.Email!);

        if (userFromDb is not null)
            return Result.Conflict(
                code: "EMP-100-001",
                type: "entity_already_exists",
                message: "Employee with this email already exists",
                detail: "Please check provided email or provide another email address"
            );

        var employeeRole = await _employeeRoleRepository.FirstOrDefaultAsync(er => er.Name == createEmployeeModel.Role!);

        if (employeeRole is null)
            return Result.NotFound(
                code: "EMP-000-003",
                type: "entity_not_found",
                message: "Employee Role not found",
                detail: "Please provide correct role name"
            );

        var passwordHash = _passwordHasher.Hash(createEmployeeModel.Password!);

        var newUser = await _userRepository.AddAsync(Tuple.Create(createEmployeeModel, passwordHash).Adapt<User>());

        if (newUser is not null)
        {
            var newEmployee = await _employeeRepository.AddAsync(new Employee { Role = employeeRole, User = newUser });

            if (newEmployee is not null)
            {
                await _cache.InsertAsync(newEmployee);
                return Result.Created(newEmployee.Adapt<EmployeeResponse>());
            }
        }

        return Result.Error(
            code: "EMP-100-002",
            type: "error_while_creation_employee",
            message: "Cannot create employee",
            detail: "Unexpected error"
        );
    }

    public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
    {
        bool isModified = false;

        var employee = await _employeeRepository.SelectByIdAsync(id);

        if (employee is null)
            return Result.NotFound(
                code: "EMP-000-001",
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
                   code: "EMP-000-002",
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
                   code: "EMP-000-002",
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
                   code: "EMP-000-002",
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
                   code: "EMP-000-002",
                   type: "invalid_model",
                   message: "One of field are not valid",
                   detail: "Check all fields and try again"
               );

            var employeeRole = await _employeeRoleRepository.FirstOrDefaultAsync(er => er.Name == updateEmployeeModel.Role!);

            if (employeeRole is null)
                return Result.NotFound(
                    code: "EMP-000-003",
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
                code: "EMP-100-003",
                type: "error_while_updating_employee",
                message: "Cannot update employee",
                detail: "Unexpected error"
            );
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.SelectByIdAsync(id);

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
            code: "EMP-100-004",
            type: "error_while_removing_employee",
            message: "Cannot remove employee",
            detail: "Unexpected error"
        );
    }
}
