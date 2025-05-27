using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockService.Application.Commands;
using StockService.Application.Queries;

namespace StockService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(string productId)
        {
            var result = await _mediator.Send(new GetStockByProductIdQuery { ProductId = productId });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllStocksQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { productId = command.ProductId }, new { Id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteProductCommand(id));
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Product with ID {id} not found.");
            }
        }

    }
}
