using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync(CancellationToken cancellationToken);
        Task AddOrderAsync(Order order);
        Task AddOutboxAsync(OutboxMessage message);
        Task SaveChangesAsync();
    }
}
