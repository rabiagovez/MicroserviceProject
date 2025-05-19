using MediatR;
using StockService.Application.Interfaces;
using StockService.Application.Models;
using StockService.Application.Queries;

namespace StockService.Application.Handlers
{
    public class GetStockByProductIdHandler : IRequestHandler<GetStockByProductIdQuery, StockDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetStockByProductIdHandler> _logger;

        public GetStockByProductIdHandler(IProductRepository productRepository, ILogger<GetStockByProductIdHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<StockDto> Handle(GetStockByProductIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProductId: {ProductId} için stok bilgisi getiriliyor...", request.ProductId);

            var product = await _productRepository.GetByProductIdAsync(request.ProductId);

            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", request.ProductId);
                return null!;
            }

            return new StockDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Stock = product.Stock
            };
        }
    }
}
