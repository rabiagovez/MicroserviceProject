using Eventing.Dtos;
using Eventing.Events;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using System.Text.Json;

namespace OrderService.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderCreatedEvent> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sipariş oluşturuluyor. BuyerId: {BuyerId}, Ürün Sayısı: {Count}", request.BuyerId, request.Items.Count);

            try
            {
                var order = new Order
                {
                    BuyerId = request.BuyerId,
                    OrderItems = request.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ProductName = "", // opsiyonel alan
                        Quantity = i.Quantity
                    }).ToList()
                };

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangesAsync();
                _logger.LogInformation("Sipariş veritabanına kaydedildi. OrderId: {OrderId}", order.Id);

                var @event = new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    BuyerId = order.BuyerId,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Items = order.OrderItems.Select(x => new OrderItemDto
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                };

                var outboxMessage = new OutboxMessage
                {
                    EventType = @event.GetType().AssemblyQualifiedName!,
                    Content = JsonSerializer.Serialize(@event, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = null
                    })
                };

                await _orderRepository.AddOutboxAsync(outboxMessage);
                await _orderRepository.SaveChangesAsync();

                _logger.LogInformation("Outbox mesajı oluşturuldu. OrderId: {OrderId}, EventType: {EventType}",
                    order.Id, outboxMessage.EventType);

                return @event;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturma sırasında hata oluştu. BuyerId: {BuyerId}", request.BuyerId);
                throw;
            }
        }
    }

}
