using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace KaspelTestTask.Persistence.Repositories;

public class OrderItemRepository: IOrderItemRepository
{
    readonly IKaspelTestTaskDbContext _dbContext;

    public OrderItemRepository(IKaspelTestTaskDbContext dbContext)
        => _dbContext = dbContext;

    public async Task RegisterOrderItemAsync(OrderItem item)
    {
        await _dbContext.OrderItems.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderItemAsync(Guid bookId, int quantity)
    {
        var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(oi => oi.BookId == bookId) ?? throw new ContentNotFoundException() ;
        orderItem.Quantity += quantity;
        _dbContext.OrderItems.Update(orderItem);
        await _dbContext.SaveChangesAsync();
    }
}

