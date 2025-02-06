using System.Security.Claims;
using Humanizer;
using Mapster;
using Restaurant.API.Mail.Constants;
using Restaurant.API.Mail.Models;
using MailTemplateModels = Restaurant.API.Mail.Templates.Models;
using SecurityModels = Restaurant.API.Security.Models;
using Restaurant.API.Models.Product;
using Restaurant.API.Models.Order;
using Restaurant.API.Models.OrderLineItem;
using Restaurant.Domain;

namespace Restaurant.API.Mapping;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ClaimsPrincipal, SecurityModels.AuthenticatedUser>()
            .Map(d => d.Name, s => s.FindFirstValue(SecurityModels.ClaimTypes.Name))
            .Map(d => d.Email, s => s.FindFirstValue(SecurityModels.ClaimTypes.Email))
            .Map(d => d.UserRole, s => Enum.Parse(typeof(UserRole), s.FindFirstValue(SecurityModels.ClaimTypes.UserRole).Dehumanize()))
            .Map(d => d.IsVerified, s => bool.Parse(s.FindFirstValue(SecurityModels.ClaimTypes.IsVerified).Humanize(LetterCasing.Title)));


        config
            .NewConfig<Tuple<Customer, string>, EmailSendMetadata<MailTemplateModels.EmailVerificationModel>>()
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
            .Map(d => d.Items, s => s.Items.Adapt<List<OrderLineItemResponse>>())
            .Map(d => d.OrderId, s => s.Id)
            .Map(d => d.Status, s => s.Status.Humanize().Underscore())
            .Map(d => d.Payment, s => s.Payment)
            .Map(d => d.CreatedAt, s => s.CreatedAt)
            .Map(d => d.UpdatedAt, s => s.UpdatedAt);
    }
}