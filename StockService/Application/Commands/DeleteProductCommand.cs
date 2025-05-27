using MediatR;

namespace StockService.Application.Commands;

public class DeleteProductCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
