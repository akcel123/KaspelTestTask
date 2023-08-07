using KaspelTestTask.Core.Domain;

namespace KaspelTestTask.Application.Interfaces;

public interface IOrderItemRepository
{
    public Task RegisterOrderItemAsync(OrderItem item);
    public Task UpdateOrderItemAsync(Guid bookId, int quantity);
}

