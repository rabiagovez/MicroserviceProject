using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Application.Queries;

namespace OrderService.Application.Handlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetAllOrdersQueryHandler> _logger;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, ILogger<GetAllOrdersQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm siparişler getiriliyor...");

            var stocks = await  _orderRepository.GetAllOrdersAsync(cancellationToken);

            _logger.LogInformation("{Count} adet sipariş listelendi.", stocks.Count);

            return stocks;
        }
    }
}
