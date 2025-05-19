using Eventing.Events;
using Eventing.Interfaces;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure;

namespace NotificationService.Application.Handlers
{
    public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
    {
        private readonly INotificationSender _notificationSender;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(
            INotificationSender notificationSender,
            INotificationRepository notificationRepository,
            ILogger<OrderCreatedEventHandler> logger)
        {
            _notificationSender = notificationSender;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            _logger.LogInformation("Bildirim kuyruğuna giriş yapıldı. OrderId: {OrderId}", @event.OrderId);

            try
            {
                await _notificationSender.SendEmailAsync(
                    @event.Email,
                    $"Sipariş Oluşturuldu: {@event.OrderId}",
                    $"Merhaba {@event.BuyerId}, siparişiniz alınmıştır."
                );
                _logger.LogInformation("E-posta gönderildi. Email: {Email}", @event.Email);

                await _notificationSender.SendSmsAsync(
                    @event.PhoneNumber,
                    $"Siparişiniz alındı: {@event.OrderId}"
                );
                _logger.LogInformation("SMS gönderildi. Telefon: {Phone}", @event.PhoneNumber);

                var log = new NotificationLog
                {
                    OrderId = @event.OrderId,
                    BuyerId = @event.BuyerId,
                    Message = $"Sipariş {@event.OrderId} için e-posta ve SMS gönderildi."
                };

                await _notificationRepository.AddLogAsync(log);
                await _notificationRepository.SaveChangesAsync();
                _logger.LogInformation("NotificationLog veritabanına kaydedildi. OrderId: {OrderId}", @event.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirim gönderimi sırasında hata oluştu. OrderId: {OrderId}", @event.OrderId);
                throw;
            }
        }
    }

}
