using Ardalis.Result;
using Restaurant.API.Models.Employee;

namespace Restaurant.API.Services.Contracts;

public interface IEmployeeService
{
    public Task<Result<List<EmployeeResponse>>> GetAllEmployeesAsync();
    public Task<Result<EmployeeResponse>> GetEmployeeByIdAsync(Guid id);
    public Task<Result<List<EmployeeResponse>>> GetEmployeeByEmailAsync(string email);
    public Task<Result<List<EmployeeResponse>>> GetEmployeeByRoleAsync(string role);
    public Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeModel createEmployeeModel);
    public Task<Result<EmployeeResponse>> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel);
    public Task<Result> RemoveEmployeeAsync(Guid id);
}
