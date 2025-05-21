using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;

namespace ECommerceCase.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            // Example order summary
            var order = new CreateOrderViewModel
            {
                BuyerId = "123",
                Email = "buyer@example.com",
                PhoneNumber = "1234567890",
                Items = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel { ProductId = "1", ProductName = "Product A", Quantity = 2 },
                    new OrderItemViewModel { ProductId = "2", ProductName = "Product B", Quantity = 1 }
                }
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CreateOrderViewModel order)
        {
            // Call Order Service and reduce stock
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}