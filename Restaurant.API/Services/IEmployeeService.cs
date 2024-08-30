using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;

namespace Restaurant.API.Services;

public interface IEmployeeService
{
    public Task<Result<List<Employee>>> GetAllEmployeesAsync();
    public Task<Result<Employee>> GetEmployeeByIdAsync(Guid id);
    public Task<Result<Employee>> GetEmployeeByEmailAsync(string email);
    public Task<Result<Employee>> GetEmployeeByRoleAsync(string role);
    public Task<Result<Employee>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest);
    public Task<Result<Employee>> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest updateEmployeeRequest);
    public Task<Result> RemoveEmployeeAsync(Guid id);
}
