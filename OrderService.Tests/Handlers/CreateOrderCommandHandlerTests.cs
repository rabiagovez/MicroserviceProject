using Eventing.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Commands;
using OrderService.Application.Handlers;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Tests.Handlers
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;
        private readonly Mock<ILogger<CreateOrderCommandHandler>> _mockLogger;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _mockLogger = new Mock<ILogger<CreateOrderCommandHandler>>();
            _handler = new CreateOrderCommandHandler(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Order_And_Return_Event()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                BuyerId = "test-buyer",
                Email = "test@mail.com",
                PhoneNumber = "+905000000000",
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = Guid.NewGuid().ToString(), Quantity = 2 }
                }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.BuyerId.Should().Be(command.BuyerId);
            result.Items.Should().HaveCount(1);

            _mockRepo.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            _mockRepo.Verify(r => r.AddOutboxAsync(It.IsAny<OutboxMessage>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
        }
    }
}
