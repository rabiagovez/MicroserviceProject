using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;

namespace ECommerceCase.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            // Example product data
            var products = new List<ProductViewModel>
            {
                new ProductViewModel { ProductId = "1", Name = "Product A", Price = 100, Stock = 10 },
                new ProductViewModel { ProductId = "2", Name = "Product B", Price = 200, Stock = 5 }
            };

            return View(products);
        }
    }
}