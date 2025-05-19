using MediatR;
using StockService.Application.Commands;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;

namespace StockService.Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductRepository productRepository, ILogger<CreateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni ürün oluşturma işlemi başladı. ProductId: {ProductId}, Name: {Name}, Stok: {Stock}",
                request.ProductId, request.Name, request.Stock);

            try
            {
                var product = new Product
                {
                    ProductId = request.ProductId,
                    Name = request.Name,
                    Stock = request.Stock
                };

                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();

                _logger.LogInformation("Yeni ürün veritabanına eklendi. Id: {Id}, ProductId: {ProductId}", product.Id, product.ProductId);

                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yeni ürün oluşturulurken hata oluştu. ProductId: {ProductId}", request.ProductId);
                throw;
            }
        }
    }

}
