using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult GetOrders()
        {
            // Example: Return a list of orders
            var orders = new List<object>
            {
                new
                {
                    buyerId = "1",
                    email = "buyer1@mail.com",
                    phoneNumber = "+905000000000",
                    items = new[]
                    {
                        new { productId = "1", productName = "Product A", quantity = 2 }
                    }
                }
            };
            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] object order)
        {
            // Example: Create order logic
            return Ok(new { message = "Order created successfully" });
        }
    }
}