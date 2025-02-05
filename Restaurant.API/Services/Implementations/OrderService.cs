using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Entities;
using Restaurant.API.Models.Order;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class OrderService(IRepository<Order> orderRepository, RestaurantDbContext dbContext, IValidator<CreateOrderModel> validator) : IOrderService
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly RestaurantDbContext _dbContext = dbContext;
    private readonly IValidator<CreateOrderModel> _validator = validator;

    public async Task<Result<List<OrderResponse>>> GetOrdersAsync(OrderQuery orderQuery)
    {
        if (orderQuery.CustomerId is not null)
            return await GetOrdersByCustomerAsync(orderQuery.CustomerId.GetValueOrDefault());

        if (orderQuery.WaiterId is not null)
            return await GetOrdersByEmployeeAsync(orderQuery.WaiterId.GetValueOrDefault());

        if (orderQuery.DeskId is not null)
            return await GetOrdersByDeskAsync(orderQuery.DeskId.GetValueOrDefault());

        if (orderQuery.Status is not null)
            return await _orderRepository.WhereAsync<OrderResponse>(o => o.Status == orderQuery.Status);

        return await _orderRepository.SelectAllAsync<OrderResponse>();
    }

    public async Task<Result<OrderResponse>> GetOrderByIdAsync(Guid orderId) =>
        Result.Success(await _orderRepository.WhereFirstAsync<OrderResponse>(o => o.Id == orderId)) ?? DetailedError.NotFound("Provide correct ID");

    public async Task<Result<List<OrderResponse>>> GetOrdersByCustomerAsync(Guid customerId) =>
        await _orderRepository.WhereAsync<OrderResponse>(o => o.Customer.Id == customerId);

    public async Task<Result<List<OrderResponse>>> GetOrdersByEmployeeAsync(Guid employeeId) =>
        await _orderRepository.WhereAsync<OrderResponse>(o => o.Waiter.Id == employeeId);

    public async Task<Result<List<OrderResponse>>> GetOrdersByDeskAsync(Guid deskId) =>
        await _orderRepository.WhereAsync<OrderResponse>(o => o.Desk.Id == deskId);

    public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderModel createOrderModel)
    {
        var validationResult = await _validator.ValidateAsync(createOrderModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == createOrderModel.CustomerId);

        if (customer is null)
            return DetailedError.NotFound("Customer not found", "Provide correct customer ID");

        var waiterRole = await _dbContext.EmployeeRoles.FirstOrDefaultAsync(er => er.Name == "waiter");

        if (waiterRole is null)
            return DetailedError.NotFound("Waiter Role not found", "Provide correct waiter role ID");

        var waiter = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == createOrderModel.WaiterId && e.Role.Id == waiterRole.Id);

        if (waiter is null)
            return DetailedError.NotFound("Waiter not found", "Provide correct employee ID");

        var desk = await _dbContext.Desks.FirstOrDefaultAsync(d => d.Id == createOrderModel.DeskId);

        if (desk is null)
            return DetailedError.NotFound("Desk not found", "Provide correct desk ID");

        List<OrderLineItem> orderItems = [];

        foreach (var (k, v) in createOrderModel.OrderItems)
        {
            orderItems.Add(new OrderLineItem { ProductId = k, Count = v });
        }

        await _dbContext.OrderLineItems.AddRangeAsync(orderItems);

        var order = new Order
        {
            Customer = customer,
            Desk = desk,
            Waiter = waiter,
            Items = orderItems
        };

        var newOrder = await _orderRepository.AddAsync(order);

        if (newOrder is null)
            return DetailedError.CreatingProblem("Cannot create new order");

        return Result.Created(newOrder.Adapt<OrderResponse>());
    }

    public async Task<Result<OrderResponse>> AddPaymentAsync(Guid orderId, Guid paymentId)
    {
        var order = await _orderRepository.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return DetailedError.NotFound("Order not found", "Provide correct order ID");

        var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment is null)
            return DetailedError.NotFound("Payment not found", "Provide correct payment ID");

        order.Payment = payment;

        if (await _orderRepository.UpdateAsync(order))
        {
            return order.Adapt<OrderResponse>();
        }

        return DetailedError.UpdatingProblem("Cannot add payment to order");
    }

    public async Task<Result> CloseOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);

        if (order is null)
            return DetailedError.NotFound("Order not found", "Provide correct order ID");

        if (order.Status == OrderStatus.Pending)
        {
            order.Status = OrderStatus.Closed;

            await _orderRepository.UpdateAsync(order);

            return Result.Success();
        }

        return DetailedError.Create(b => b
            .WithStatus(ResultStatus.Error)
            .WithSeverity(ErrorSeverity.Warning)
            .WithType("CLOSE_ORDER_PROBLEM")
            .WithTitle("Cannot close order")
            .WithMessage("Unexpected error")
        );
    }
}