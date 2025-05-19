using Eventing.Dtos;

namespace Eventing.Events
{
    public class OrderCreatedEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
