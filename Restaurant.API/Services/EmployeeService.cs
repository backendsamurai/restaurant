using Ardalis.Result;
using Restaurant.API.Entities;
using Restaurant.API.Models.Employee;

namespace Restaurant.API.Services;

public sealed class EmployeeService : IEmployeeService
{
    public Task<Result<Employee>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<Employee>>> GetAllEmployeesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Employee>> GetEmployeeByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Employee>> GetEmployeeByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Employee>> GetEmployeeByRoleAsync(string role)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveEmployeeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Employee>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
    {
        throw new NotImplementedException();
    }
}
