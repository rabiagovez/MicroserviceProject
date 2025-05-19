using Microsoft.EntityFrameworkCore;
using StockService.Application.Interfaces;
using StockService.Application.Models;
using StockService.Domain.Entities;

namespace StockService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StockDbContext _context;

        public ProductRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockDto>> GetAllStocksAsync(CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new StockDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Stock = p.Stock
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<Product?> GetByProductIdAsync(string productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
