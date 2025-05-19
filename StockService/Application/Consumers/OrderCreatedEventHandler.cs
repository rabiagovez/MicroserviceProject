using Eventing.Events;
using Eventing.Interfaces;
using StockService.Application.Interfaces;

namespace StockService.Application.Consumers
{
    public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(IProductRepository productRepository, ILogger<OrderCreatedEventHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            _logger.LogInformation("Stock güncellemesi başladı. OrderId: {OrderId}, Ürün Sayısı: {Count}",
                @event.OrderId, @event.Items.Count);

            try
            {
                foreach (var item in @event.Items)
                {
                    var product = await _productRepository.GetByProductIdAsync(item.ProductId);

                    if (product is null)
                    {
                        _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", item.ProductId);
                        continue;
                    }

                    var oldStock = product.Stock;
                    product.Stock -= item.Quantity;

                    _logger.LogInformation("Stok güncellendi. ProductId: {ProductId}, Önceki Stok: {OldStock}, Yeni Stok: {NewStock}",
                        product.ProductId, oldStock, product.Stock);
                }

                await _productRepository.SaveChangesAsync();
                _logger.LogInformation("StockService: Veritabanı güncellemeleri tamamlandı. OrderId: {OrderId}", @event.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok güncellemesi sırasında hata oluştu. OrderId: {OrderId}", @event.OrderId);
                throw;
            }
        }
    }

}
