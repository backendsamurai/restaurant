using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;
using Restaurant.Persistence;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.Order;

namespace Restaurant.Services.Implementations;

public sealed class OrdersService : IOrdersService
{
    private readonly RestaurantDbContext _dbContext;

    private readonly IConsumersService _consumersService;

    public OrdersService(RestaurantDbContext dbContext, IConsumersService consumersService)
    {
        _dbContext = dbContext;
        _consumersService = consumersService;
    }

    public async Task<Result<List<Order>>> GetOrdersAsync(Guid? consumerId = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Order> queryable = _dbContext.Orders.AsNoTracking().AsQueryable();

        if (consumerId is not null && consumerId != Guid.Empty)
        {
            queryable = queryable.Where(o => o.Consumer != null && o.Consumer.Id == consumerId);
        }

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<Result<Order>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Result.NotFound();

        return Result.Success(order);
    }

    public async Task<Result<Order>> CreateOrderAsync(CreateOrderDTO createOrderDTO, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var consumer = await _consumersService.GetConsumerAsync(createOrderDTO.ConsumerId, cancellationToken);

            if (consumer.IsError())
                return Result.Error();

            var order = new Order(Guid.NewGuid(), consumer.Value);

            List<OrderLineItem> lineItems = [];

            foreach (var lineItem in createOrderDTO.LineItems)
            {
                var item = new OrderLineItem(Guid.NewGuid(), lineItem.Key, lineItem.Value);
                lineItems.Add(item);
                order.AddItemToOrder(item);
            }

            await _dbContext.OrderLineItems.AddRangeAsync(lineItems, cancellationToken);
            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Created(order);
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }

    public async Task<Result> CancelOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order is null)
                return Result.NotFound();

            order.ChangeStatus(OrderStatus.Cancelled);

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            await tx.RollbackAsync(cancellationToken);
            return Result.Error();
        }
    }
}
