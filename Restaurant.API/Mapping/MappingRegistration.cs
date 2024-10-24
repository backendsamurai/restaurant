using System.Security.Claims;
using Humanizer;
using Mapster;
using Restaurant.API.Entities;
using Restaurant.API.Mail.Constants;
using Restaurant.API.Mail.Models;
using MailTemplateModels = Restaurant.API.Mail.Templates.Models;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Employee;
using Restaurant.API.Models.User;
using Restaurant.API.Services.Implementations;
using SecurityModels = Restaurant.API.Security.Models;
using Restaurant.API.Models.Product;
using Restaurant.API.Caching.Models;
using Restaurant.API.Models.Order;
using Restaurant.API.Models.OrderLineItem;

namespace Restaurant.API.Mapping;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Customer, CustomerResponse>()
            .Map(d => d.CustomerId, s => s.Id)
            .Map(d => d.UserId, s => s.User.Id)
            .Map(d => d.UserName, s => s.User.Name)
            .Map(d => d.UserEmail, s => s.User.Email)
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
            .Map(d => d.EmployeeRole, s => s.FindFirstValue(SecurityModels.ClaimTypes.EmployeeRole))
            .Map(d => d.IsVerified, s => bool.Parse(s.FindFirstValue(SecurityModels.ClaimTypes.IsVerified).Humanize(LetterCasing.Title)));

        config
            .NewConfig<Employee, EmployeeResponse>()
            .Map(d => d.EmployeeId, s => s.Id)
            .Map(d => d.UserId, s => s.User.Id)
            .Map(d => d.UserName, s => s.User.Name)
            .Map(d => d.UserEmail, s => s.User.Email)
            .Map(d => d.IsVerified, s => s.User.IsVerified)
            .Map(d => d.EmployeeRole, s => s.Role.Name);

        config
            .NewConfig<Employee, EmployeeCacheModel>()
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

        config
            .NewConfig<Customer, UserInfo>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.User, s => s.User)
            .Map(d => d.EmployeeRole, _ => string.Empty);

        config
            .NewConfig<Employee, UserInfo>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.User, s => s.User)
            .Map(d => d.EmployeeRole, s => s.Role.Name);

        config
            .NewConfig<Tuple<User, string>, EmailSendMetadata<MailTemplateModels.EmailVerificationModel>>()
            .Map(d => d.RecipientEmail, s => s.Item1.Email)
            .Map(d => d.Subject, _ => EmailSubjects.VERIFICATION_SUBJECT)
            .Map(d => d.TemplateFileName, _ => EmailTemplates.VERIFICATION)
            .Map(d => d.TemplateModel, s => new MailTemplateModels.EmailVerificationModel(s.Item1.Id, s.Item1.Name, s.Item2));

        config
            .NewConfig<CreateProductModel, Product>()
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Description, s => s.Description)
            .Map(d => d.ImageUrl, s => "")
            .Map(d => d.Price, s => s.Price);

        config
            .NewConfig<OrderLineItem, OrderLineItemResponse>()
            .Map(d => d.ProductName, s => s.Product!.Name)
            .Map(d => d.ProductPrice, s => s.Product!.Price)
            .Map(d => d.Count, s => s.Count);

        config
            .NewConfig<Order, OrderResponse>()
            .Map(d => d.CustomerId, s => s.Customer.Id)
            .Map(d => d.WaiterId, s => s.Waiter.Id)
            .Map(d => d.DeskId, s => s.Desk.Id)
            .Map(d => d.Items, s => s.Items.Adapt<List<OrderLineItemResponse>>())
            .Map(d => d.OrderId, s => s.Id)
            .Map(d => d.Status, s => s.Status.Humanize().Underscore())
            .Map(d => d.Payment, s => s.Payment)
            .Map(d => d.CreatedAt, s => s.CreatedAt)
            .Map(d => d.UpdatedAt, s => s.UpdatedAt);
    }
}