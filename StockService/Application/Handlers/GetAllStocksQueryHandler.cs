using MediatR;
using StockService.Application.Interfaces;
using StockService.Application.Models;
using StockService.Application.Queries;

namespace StockService.Application.Handlers
{
    public class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, List<StockDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllStocksQueryHandler> _logger;

        public GetAllStocksQueryHandler(IProductRepository productRepository, ILogger<GetAllStocksQueryHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<StockDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm ürün stokları getiriliyor...");

            var stocks = await _productRepository.GetAllStocksAsync(cancellationToken);

            _logger.LogInformation("{Count} adet ürün listelendi.", stocks.Count);

            return stocks;
        }
    }
}
