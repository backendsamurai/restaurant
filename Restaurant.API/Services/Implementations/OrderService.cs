using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Models.Order;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Services.Implementations;

public sealed class OrderService(
    IRepository<Order> orderRepository,
    RestaurantDbContext dbContext,
    IValidator<CreateOrderModel> validator) : IOrderService
{
    public async Task<Result<List<OrderResponse>>> GetOrdersAsync(OrderQuery orderQuery)
    {
        if (orderQuery.CustomerId is not null)
            return await GetOrdersByCustomerAsync(orderQuery.CustomerId.GetValueOrDefault());

        if (orderQuery.Status is not null)
            return await orderRepository.WhereAsync<OrderResponse>(o => o.Status == orderQuery.Status);

        return await orderRepository.SelectAllAsync<OrderResponse>();
    }

    public async Task<Result<OrderResponse>> GetOrderByIdAsync(Guid orderId) =>
        Result.Success(await orderRepository.WhereFirstAsync<OrderResponse>(o => o.Id == orderId)) ?? DetailedError.NotFound("Provide correct ID");

    public async Task<Result<List<OrderResponse>>> GetOrdersByCustomerAsync(Guid customerId) =>
        await orderRepository.WhereAsync<OrderResponse>(o => o.Customer.Id == customerId);

    public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderModel createOrderModel)
    {
        var validationResult = await validator.ValidateAsync(createOrderModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == createOrderModel.CustomerId);

        if (customer is null)
            return DetailedError.NotFound("Customer not found", "Provide correct customer ID");

        var order = new Order(customer);

        foreach (var (productId, count) in createOrderModel.OrderItems)
        {
            order.AddItemToOrder(new OrderLineItem(productId, count));
        }

        var newOrder = await orderRepository.AddAsync(order);

        if (newOrder is null)
            return DetailedError.CreatingProblem("Cannot create new order");

        return Result.Created(newOrder.Adapt<OrderResponse>());
    }

    public async Task<Result> CancelOrderAsync(Guid orderId)
    {
        var order = await orderRepository.SelectByIdAsync(orderId);

        if (order is null)
            return DetailedError.NotFound("Order not found", "Provide correct order ID");

        if (order.Status == OrderStatus.Pending)
        {
            order.ChangeStatus(OrderStatus.Cancelled);

            await orderRepository.UpdateAsync(order);

            return Result.Success();
        }

        return DetailedError.Create(b => b
            .WithStatus(ResultStatus.Error)
            .WithSeverity(ErrorSeverity.Warning)
            .WithType("CANCEL_ORDER_ERROR")
            .WithTitle("Cannot close order")
            .WithMessage("Unexpected error")
        );
    }
}