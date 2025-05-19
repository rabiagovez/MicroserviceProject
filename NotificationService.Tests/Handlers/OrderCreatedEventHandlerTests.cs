using Eventing.Dtos;
using Eventing.Events;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Handlers;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;

namespace NotificationService.Tests.Handlers
{
    public class OrderCreatedEventHandlerTests
    {
        private readonly Mock<INotificationSender> _mockSender;
        private readonly Mock<INotificationRepository> _mockRepo;
        private readonly Mock<ILogger<OrderCreatedEventHandler>> _mockLogger;
        private readonly OrderCreatedEventHandler _handler;

        public OrderCreatedEventHandlerTests()
        {
            _mockSender = new Mock<INotificationSender>();
            _mockRepo = new Mock<INotificationRepository>();
            _mockLogger = new Mock<ILogger<OrderCreatedEventHandler>>();
            _handler = new OrderCreatedEventHandler(_mockSender.Object, _mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Send_Email_And_Sms_And_Log()
        {
            // Arrange
            var @event = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                BuyerId = "user-123",
                Email = "test@example.com",
                PhoneNumber = "+905000000000",
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = Guid.NewGuid().ToString(), ProductName="testname", Quantity = 1 }
                }
            };

            // Act
            await _handler.Handle(@event);

            // Assert
            _mockSender.Verify(s => s.SendEmailAsync(@event.Email,
                It.Is<string>(subject => subject.Contains(@event.OrderId.ToString())),
                It.Is<string>(body => body.Contains(@event.BuyerId))), Times.Once);

            _mockSender.Verify(s => s.SendSmsAsync(@event.PhoneNumber,
                It.Is<string>(body => body.Contains(@event.OrderId.ToString()))), Times.Once);

            _mockRepo.Verify(r => r.AddLogAsync(It.Is<NotificationLog>(
                l => l.OrderId == @event.OrderId && l.BuyerId == @event.BuyerId
            )), Times.Once);

            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
