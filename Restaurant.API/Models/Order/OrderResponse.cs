using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Employee;

namespace Restaurant.API.Models.Order;

public sealed record OrderResponse(
    Guid OrderId,
    CustomerResponse Customer,
    EmployeeResponse Waiter,
    Entities.Desk Desk,
    List<OrderLineItem> OrderLineItems,
    OrderStatus OrderStatus,
    Entities.Payment Payment,
    DateTime CreatedAt,
    DateTime UpdatedAt
);