using Microsoft.AspNetCore.Mvc;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        [HttpGet]
        public IActionResult GetStocks()
        {
            // Example: Return a list of stocks
            var stocks = new List<object>
            {
                new { productId = "1", name = "Product A", stock = 10 },
                new { productId = "2", name = "Product B", stock = 5 }
            };
            return Ok(stocks);
        }

        [HttpPost]
        public IActionResult AddStock([FromBody] object stock)
        {
            // Example: Add stock logic
            return Ok(new { message = "Stock added successfully" });
        }
    }
}