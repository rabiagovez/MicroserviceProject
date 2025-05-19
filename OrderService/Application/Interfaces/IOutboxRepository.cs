using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task<List<OutboxMessage>> GetUnpublishedMessagesAsync(CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
