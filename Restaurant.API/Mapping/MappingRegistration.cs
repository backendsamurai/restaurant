using System.Security.Claims;
using Humanizer;
using Mapster;
using Restaurant.Domain;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Order;
using Restaurant.Shared.Models.OrderLineItem;
using Restaurant.Shared.Models.Product;

namespace Restaurant.API.Mapping;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ClaimsPrincipal, AuthenticatedUser>()
            .Map(d => d.Name, s => s.FindFirstValue(Shared.Models.ClaimTypes.Name))
            .Map(d => d.Email, s => s.FindFirstValue(Shared.Models.ClaimTypes.Email))
            .Map(d => d.UserRole, s => Enum.Parse(typeof(UserRole), s.FindFirstValue(Shared.Models.ClaimTypes.UserRole).Dehumanize()));

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