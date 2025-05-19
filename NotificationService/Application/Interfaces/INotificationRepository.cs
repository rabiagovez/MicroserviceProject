using NotificationService.Domain.Entities;

namespace NotificationService.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task AddLogAsync(NotificationLog log);
        Task SaveChangesAsync();
    }
}
