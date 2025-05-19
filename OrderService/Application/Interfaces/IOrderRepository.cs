using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task AddOutboxAsync(OutboxMessage message);
        Task SaveChangesAsync();
    }
}
