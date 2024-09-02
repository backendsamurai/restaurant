using Ardalis.Result;
using Restaurant.API.Entities;
using Restaurant.API.Models.EmployeeRole;

namespace Restaurant.API.Services.Contracts;

public interface IEmployeeRoleService
{
    public Task<Result<List<EmployeeRole>>> GetAllEmployeeRolesAsync();
    public Task<Result<EmployeeRole>> GetEmployeeRoleByIdAsync(Guid id);
    public Task<Result<List<EmployeeRole>>> GetEmployeeRoleByNameAsync(string name);
    public Task<Result<EmployeeRole>> CreateEmployeeRoleAsync(CreateEmployeeRoleModel createEmployeeRoleModel);
    public Task<Result<EmployeeRole>> UpdateEmployeeRoleAsync(Guid id, UpdateEmployeeRoleModel updateEmployeeRoleModel);
    public Task<Result> RemoveEmployeeRoleAsync(Guid id);
}
