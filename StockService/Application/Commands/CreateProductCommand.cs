using MediatR;

namespace StockService.Application.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
