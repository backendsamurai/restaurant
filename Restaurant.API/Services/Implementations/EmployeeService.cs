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

        return employee is null ? DetailedError.NotFound("Please provide correct id") : Result.Success(employee);
    }

    public async Task<Result<List<EmployeeResponse>>> GetEmployeeByRoleAsync(string role) =>
        await _cache.GetOrSetAsync(e => e.EmployeeRole.StartsWith(role),
            async () => await _employeeRepository.WhereAsync<EmployeeResponse>(e => e.Role.Name.StartsWith(role)));

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel)
    {
        var validationResult = await _createEmployeeModelValidator.ValidateAsync(createEmployeeModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var userFromDb = await _userRepository.FirstOrDefaultAsync(u => u.Email == createEmployeeModel.Email!);

        if (userFromDb is not null)
            return DetailedError.Conflict("Employee with this email already exists", "Please check provided email or provide another email address");

        var employeeRole = await _employeeRoleRepository.FirstOrDefaultAsync(er => er.Name == createEmployeeModel.Role!);

        if (employeeRole is null)
            return DetailedError.NotFound("Please provide correct id");

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

        return DetailedError.CreatingProblem("Cannot create employee", "Unexpected error");
    }

    public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
    {
        bool isModified = false;

        var employee = await _employeeRepository.SelectByIdAsync(id);

        if (employee is null)
            return DetailedError.NotFound("Please provide correct id");

        if (updateEmployeeModel.Name is not null && updateEmployeeModel.Name != employee.User.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("name"));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid name", validationResult.Errors.First().ErrorMessage);

            employee.User.Name = updateEmployeeModel.Name;
            isModified = true;
        }

        if (updateEmployeeModel.Email is not null && updateEmployeeModel.Email != employee.User.Email)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("email"));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid email", validationResult.Errors.First().ErrorMessage);

            employee.User.Email = updateEmployeeModel.Email;
            isModified = true;
        }

        if (updateEmployeeModel.Password is not null && !_passwordHasher.Verify(updateEmployeeModel.Password, employee.User.PasswordHash))
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("password"));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid password", validationResult.Errors.First().ErrorMessage);

            employee.User.PasswordHash = _passwordHasher.Hash(updateEmployeeModel.Password);
            isModified = true;
        }

        if (updateEmployeeModel.Role is not null && updateEmployeeModel.Role != employee.Role.Name)
        {
            var validationResult = await _updateEmployeeModelValidator
                .ValidateAsync(updateEmployeeModel, options => options.IncludeProperties("role"));

            if (!validationResult.IsValid)
                return DetailedError.Invalid("Invalid role name", validationResult.Errors.First().ErrorMessage);

            var employeeRole = await _employeeRoleRepository.FirstOrDefaultAsync(er => er.Name == updateEmployeeModel.Role!);

            if (employeeRole is null)
                return DetailedError.NotFound("Employee role not found", "Please provide correct role name");

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

            return DetailedError.UpdatingProblem("Cannot update employee", "Unexpected error");
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.SelectByIdAsync(id);

        if (employee is null)
            return DetailedError.NotFound("Please provide correct id");

        var isRemoved = await _employeeRepository.RemoveAsync(employee);

        if (isRemoved)
        {
            await _cache.DeleteAsync(employee);
            return Result.NoContent();
        }

        return DetailedError.RemoveProblem("Cannot remove employee", "Unexpected error");
    }
}
