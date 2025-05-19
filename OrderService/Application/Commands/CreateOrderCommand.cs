using Eventing.Dtos;
using Eventing.Events;
using MediatR;

namespace OrderService.Application.Commands
{
    public class CreateOrderCommand : IRequest<OrderCreatedEvent>
    {
        public string BuyerId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
