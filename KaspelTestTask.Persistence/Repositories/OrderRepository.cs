using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace KaspelTestTask.Persistence.Repositories;

public class OrderRepository: IOrderRepository
{
    readonly IKaspelTestTaskDbContext _dbContext;
    readonly IMapper _mapper;

    public OrderRepository(IKaspelTestTaskDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task AddOrderAsync(Order order)
    {
        order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var entity = await _dbContext.Orders.Where(ord => ord.Id == id).FirstOrDefaultAsync() ?? throw new ContentNotFoundException();
        _dbContext.Orders.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderInformation>> GetAllOrdersAsync()
    {
        var orders = await _dbContext.Orders.Include(order => order.OrderItems).ToListAsync();
        List<OrderInformation> ordersInformation = new();
        foreach (var order in orders)
        {
            var orderInformation = _mapper.Map<OrderInformation>(order);

            foreach (var orderItem in order.OrderItems)
                orderInformation.OrderItems.Add(new OrderItemInformation() { BookId = orderItem.BookId, Quantity = orderItem.Quantity });

            ordersInformation.Add(orderInformation);
        }
        return ordersInformation;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        return await _dbContext.Orders.Include(order => order.OrderItems).FirstOrDefaultAsync(ord => ord.Id == id);
    }

    public async Task<IEnumerable<OrderInformation>> GetOrdersByFilterAsync(Guid? id, DateTime? orderDate)
    {
        var query = _dbContext.Orders.Include(order => order.OrderItems).AsQueryable();
        if (id.HasValue)
        {
            query = query.Where(order => order.Id == id);
        }

        if (orderDate.HasValue)
        {
            query = query.Where(order => order.OrderDate.Date == orderDate.Value.Date);
        }

        var orders = await query.ToListAsync();
        var stock = await _dbContext.Stock.ToListAsync();
        List<OrderInformation> ordersInformation = new();
        foreach (var order in orders)
        {
            var orderInformation = _mapper.Map<OrderInformation>(order);

            foreach (var orderItem in order.OrderItems)
                orderInformation.OrderItems.Add(new OrderItemInformation() { BookId = orderItem.BookId, Quantity = orderItem.Quantity });

            ordersInformation.Add(orderInformation);
        }

        return ordersInformation;
    }

    public async Task SaveOrderByIdAsync(Guid guid)
    {
        var entity = await _dbContext.Orders.FirstOrDefaultAsync(opt => opt.Id == guid) ?? throw new ContentNotFoundException();
        entity.IsSaved = true;
        await _dbContext.SaveChangesAsync();
    }
}

