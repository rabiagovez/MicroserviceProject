using MediatR;
using StockService.Application.Models;

namespace StockService.Application.Queries
{
    public class GetAllStocksQuery : IRequest<List<StockDto>> { }
}
