using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;

namespace Restaurant.API.Services;

public interface IEmployeeRoleService
{
    public Task<Result<List<EmployeeRole>>> GetAllEmployeeRolesAsync();
    public Task<Result<EmployeeRole>> GetEmployeeRoleByIdAsync(Guid id);
    public Task<Result<List<EmployeeRole>>> GetEmployeeRoleByNameAsync(string name);
    public Task<Result<EmployeeRole>> CreateEmployeeRoleAsync(CreateEmployeeRoleRequest createEmployeeRoleRequest);
    public Task<Result<EmployeeRole>> UpdateEmployeeRoleAsync(Guid id, UpdateEmployeeRoleRequest updateEmployeeRoleRequest);
    public Task<Result> RemoveEmployeeRoleAsync(Guid id);
}
