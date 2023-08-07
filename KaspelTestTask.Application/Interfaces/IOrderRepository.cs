using KaspelTestTask.Application.Classes;
using KaspelTestTask.Core.Domain;

namespace KaspelTestTask.Application.Interfaces;

public interface IOrderRepository
{
    public Task<IEnumerable<OrderInformation>> GetAllOrdersAsync();
    public Task<Order?> GetOrderByIdAsync(Guid id);
    public Task AddOrderAsync(Order order);
    public Task DeleteOrderAsync(Guid id);

    public Task<IEnumerable<OrderInformation>> GetOrdersByFilterAsync(Guid? id, DateTime? orderDate);
    public Task SaveOrderByIdAsync(Guid guid);
}

