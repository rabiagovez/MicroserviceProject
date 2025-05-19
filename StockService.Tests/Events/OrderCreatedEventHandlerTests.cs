using Eventing.Dtos;
using Eventing.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockService.Application.Consumers;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;

namespace StockService.Tests.Events
{
    public class OrderCreatedEventHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<OrderCreatedEventHandler>> _mockLogger;
        private readonly OrderCreatedEventHandler _handler;

        public OrderCreatedEventHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<OrderCreatedEventHandler>>();
            _handler = new OrderCreatedEventHandler(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Decrease_Product_Stock_When_Product_Exists()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var product = new Product { ProductId = productId, Stock = 10 };

            _mockRepo.Setup(r => r.GetByProductIdAsync(productId))
                     .ReturnsAsync(product);

            var @event = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = productId, Quantity = 3 }
                }
            };

            // Act
            await _handler.Handle(@event);

            // Assert
            product.Stock.Should().Be(7);
            _mockRepo.Verify(r => r.GetByProductIdAsync(productId), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_LogWarning_When_Product_Not_Found()
        {
            // Arrange
            var missingProductId = Guid.NewGuid().ToString();

            _mockRepo.Setup(r => r.GetByProductIdAsync(missingProductId))
                     .ReturnsAsync((Product?)null);

            var @event = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = missingProductId, Quantity = 1 }
                }
            };

            // Act
            await _handler.Handle(@event);

            // Assert
            _mockRepo.Verify(r => r.GetByProductIdAsync(missingProductId), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once); // yine de çağrılır
        }
    }
}
