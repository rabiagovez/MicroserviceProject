using MediatR;
using StockService.Application.Commands;
using StockService.Application.Interfaces;

namespace StockService.Application.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByProductIdAsync(request.Id.ToString());

        if (product is null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

        await _productRepository.DeleteAsync(product);
    }
}