using System.Security.Claims;
using Humanizer;
using Mapster;
using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Employee;
using Restaurant.API.Models.User;
using SecurityModels = Restaurant.API.Security.Models;

namespace Restaurant.API.Mapping;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Customer, CustomerResponse>()
            .Map(d => d.UserId, s => s.User.Id)
            .Map(d => d.Name, s => s.User.Name)
            .Map(d => d.Email, s => s.User.Email)
            .Map(d => d.IsVerified, s => s.User.IsVerified);

        config
            .NewConfig<Tuple<User, Guid, string, string>, LoginUserResponse>()
            .Map(d => d.Id, s => s.Item2)
            .Map(d => d.UserId, s => s.Item1.Id)
            .Map(d => d.UserName, s => s.Item1.Name)
            .Map(d => d.UserEmail, s => s.Item1.Email)
            .Map(d => d.IsVerified, s => s.Item1.IsVerified)
            .Map(d => d.AccessToken, s => s.Item3)
            .Map(d => d.EmployeeRole, s => string.IsNullOrEmpty(s.Item4) ? null : s.Item4);

        config
            .NewConfig<ClaimsPrincipal, SecurityModels.AuthenticatedUser>()
            .Map(d => d.Name, s => s.FindFirstValue(SecurityModels.ClaimTypes.Name))
            .Map(d => d.Email, s => s.FindFirstValue(SecurityModels.ClaimTypes.Email))
            .Map(d => d.UserRole, s => Enum.Parse(typeof(UserRole), s.FindFirstValue(SecurityModels.ClaimTypes.UserRole).Dehumanize()))
            .Map(d => d.EmployeeRole, s => s.FindFirstValue(SecurityModels.ClaimTypes.EmployeeRole));

        config
            .NewConfig<Employee, EmployeeResponse>()
            .Map(d => d.EmployeeId, s => s.Id)
            .Map(d => d.UserId, s => s.User.Id)
            .Map(d => d.UserName, s => s.User.Name)
            .Map(d => d.UserEmail, s => s.User.Email)
            .Map(d => d.IsVerified, s => s.User.IsVerified)
            .Map(d => d.EmployeeRole, s => s.Role.Name);

        config
            .NewConfig<Tuple<CreateEmployeeModel, string>, User>()
            .Map(d => d.Name, s => s.Item1.Name)
            .Map(d => d.Email, s => s.Item1.Email)
            .Map(d => d.PasswordHash, s => s.Item2)
            .Map(d => d.Role, s => UserRole.Employee);
    }
}