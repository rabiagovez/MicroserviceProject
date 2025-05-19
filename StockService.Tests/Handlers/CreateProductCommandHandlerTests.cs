using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockService.Application.Commands;
using StockService.Application.Handlers;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;

namespace StockService.Tests.Handlers
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<CreateProductCommandHandler>> _mockLogger;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<CreateProductCommandHandler>>();
            _handler = new CreateProductCommandHandler(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Product_And_Return_Id()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = "Test Ürün",
                Stock = 100
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBe(Guid.Empty);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Product>(p =>
                p.ProductId == command.ProductId &&
                p.Name == command.Name &&
                p.Stock == command.Stock
            )), Times.Once);

            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
