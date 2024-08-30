using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;

namespace Restaurant.API.Services;

public sealed class EmployeeService : IEmployeeService
{
    public Task<Result<Employee>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest)
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

    public Task<Result<Employee>> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest updateEmployeeRequest)
    {
        throw new NotImplementedException();
    }
}
