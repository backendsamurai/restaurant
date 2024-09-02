using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;
using Restaurant.API.Models.Employee;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Services.Implementations;

public sealed class EmployeeService(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleRepository employeeRoleRepository,
    IPasswordHasherService passwordHasher,
    IValidator<CreateEmployeeModel> createEmployeeModelValidator,
    IValidator<UpdateEmployeeModel> updateEmployeeModelValidator
) : IEmployeeService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository = employeeRoleRepository;
    private readonly IPasswordHasherService _passwordHasher = passwordHasher;
    private readonly IValidator<CreateEmployeeModel> _createEmployeeModelValidator = createEmployeeModelValidator;
    private readonly IValidator<UpdateEmployeeModel> _updateEmployeeModelValidator = updateEmployeeModelValidator;

    public async Task<Result<List<EmployeeResponse>>> GetAllEmployeesAsync() =>
        await _employeeRepository
            .SelectAll()
            .ProjectToType<EmployeeResponse>()
            .ToListAsync();

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByEmailAsync(string email) =>
         await _employeeRepository
            .SelectByEmail(email)
            .ProjectToType<EmployeeResponse>()
            .ToListAsync();

    public async Task<Result<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository
            .SelectById(id)
            .ProjectToType<EmployeeResponse>()
            .FirstOrDefaultAsync();

        return employee is null ? Result.NotFound("employee not found") : Result.Success(employee);
    }

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByRoleAsync(string role) =>
        await _employeeRepository
            .SelectByRole(role)
            .ProjectToType<EmployeeResponse>()
            .ToListAsync();

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel)
    {
        var validationResult = await _createEmployeeModelValidator.ValidateAsync(createEmployeeModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var userFromDb = await _userRepository
            .SelectByEmail(createEmployeeModel.Email!)
            .ProjectToType<User>()
            .FirstOrDefaultAsync();

        if (userFromDb is not null)
            return Result.Conflict("employee with this email already exists");

        var employeeRole = await _employeeRoleRepository
            .SelectByName(createEmployeeModel.Role!)
            .ProjectToType<EmployeeRole>()
            .FirstOrDefaultAsync();

        if (employeeRole is null)
            return Result.Error("cannot find role by name, please create role");

        var passwordHash = _passwordHasher.Hash(createEmployeeModel.Password!);

        var newUser = await _userRepository.AddAsync(Tuple.Create(createEmployeeModel, passwordHash).Adapt<User>());

        if (newUser is null)
            return Result.Error("cannot create employee");

        var newEmployee = await _employeeRepository.AddAsync(newUser, employeeRole);

        return newEmployee is null
            ? Result.Error("cannot create employee")
            : Result.Success(newEmployee.Adapt<EmployeeResponse>());
    }

    public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
    {
        bool isModified = false;

        var employee = await _employeeRepository
            .SelectById(id)
            .ProjectToType<Employee>()
            .FirstOrDefaultAsync();

        if (employee is null)
            return Result.NotFound("employee not found");

        if (updateEmployeeModel.Name is not null && updateEmployeeModel.Name != employee.User.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("name"));

            if (!validationResult.IsValid)
                return Result.Invalid(validationResult.AsErrors());

            employee.User.Name = updateEmployeeModel.Name;
            isModified = true;
        }

        if (updateEmployeeModel.Email is not null && updateEmployeeModel.Email != employee.User.Email)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("email"));

            if (!validationResult.IsValid)
                return Result.Invalid(validationResult.AsErrors());

            employee.User.Email = updateEmployeeModel.Email;
            isModified = true;
        }

        if (updateEmployeeModel.Password is not null && !_passwordHasher.Verify(updateEmployeeModel.Password, employee.User.PasswordHash))
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("password"));

            if (!validationResult.IsValid)
                return Result.Invalid(validationResult.AsErrors());

            employee.User.PasswordHash = _passwordHasher.Hash(updateEmployeeModel.Password);
            isModified = true;
        }

        if (updateEmployeeModel.Role is not null && updateEmployeeModel.Role != employee.Role.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("role"));

            if (!validationResult.IsValid)
                return Result.Invalid(validationResult.AsErrors());

            var employeeRole = await _employeeRoleRepository
                .SelectByName(updateEmployeeModel.Role)
                .ProjectToType<EmployeeRole>()
                .FirstOrDefaultAsync();

            if (employeeRole is null)
                return Result.Error("the employee role cannot be updated because it does not exist");

            employee.Role = employeeRole;
            isModified = true;
        }

        if (isModified)
        {
            var isUpdated = await _employeeRepository.UpdateAsync(employee);
            return isUpdated ? Result.Success(employee.Adapt<EmployeeResponse>()) : Result.Error("cannot update employee");
        }

        return Result.Error("don`t need update employee");
    }

    public async Task<Result> RemoveEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository
            .SelectById(id)
            .ProjectToType<Employee>()
            .FirstOrDefaultAsync();

        if (employee is null)
            return Result.NotFound("employee not found");

        var isRemoved = await _employeeRepository.RemoveAsync(employee);

        return isRemoved ? Result.Success() : Result.Error("cannot remove employee");
    }
}
