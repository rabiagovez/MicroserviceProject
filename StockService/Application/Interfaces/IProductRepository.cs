using StockService.Application.Models;
using StockService.Domain.Entities;

namespace StockService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<StockDto>> GetAllStocksAsync(CancellationToken cancellationToken);
        Task<Product?> GetByProductIdAsync(string productId);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
    }
}
