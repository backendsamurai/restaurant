using Ardalis.Result;
using Restaurant.API.Entities;
using Restaurant.API.Models.Employee;

namespace Restaurant.API.Services;

public interface IEmployeeService
{
    public Task<Result<List<Employee>>> GetAllEmployeesAsync();
    public Task<Result<Employee>> GetEmployeeByIdAsync(Guid id);
    public Task<Result<Employee>> GetEmployeeByEmailAsync(string email);
    public Task<Result<Employee>> GetEmployeeByRoleAsync(string role);
    public Task<Result<Employee>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel);
    public Task<Result<Employee>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel);
    public Task<Result> RemoveEmployeeAsync(Guid id);
}
