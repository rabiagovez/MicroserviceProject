using MediatR;
using StockService.Application.Models;

namespace StockService.Application.Queries
{
    public class GetStockByProductIdQuery : IRequest<StockDto>
    {
        public string ProductId { get; set; }
    }
}
