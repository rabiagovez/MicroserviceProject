using Eventing.Events;
using Eventing.Interfaces;
using OrderService.Application.Interfaces;
using System.Text.Json;

namespace OrderService.Application.Services
{
    public class OutboxPublisherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventBus _eventBus;
        private readonly ILogger<OutboxPublisherService> _logger;

        public OutboxPublisherService(IServiceProvider serviceProvider, IEventBus eventBus, ILogger<OutboxPublisherService> logger)
        {
            _serviceProvider = serviceProvider;
            _eventBus = eventBus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OutboxPublisherService başlatıldı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                    var messages = await repository.GetUnpublishedMessagesAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            var integrationEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message.Content);
                            if (integrationEvent is null)
                            {
                                _logger.LogWarning("Deserialize başarısız. İçerik: {Content}", message.Content);
                                continue;
                            }

                            _logger.LogInformation("Event deserialized: {Content}", message.Content);
                            _logger.LogInformation("Yayınlanıyor: {OrderId}", integrationEvent.OrderId);

                            _eventBus.Publish(integrationEvent);
                            message.IsPublished = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Mesaj yayınlanırken hata oluştu.");
                        }
                    }

                    await repository.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox işlemleri sırasında genel hata oluştu.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
