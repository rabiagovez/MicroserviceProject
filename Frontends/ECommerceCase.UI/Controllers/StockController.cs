using ECommerceCase.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCase.UI.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            // Example stock data
            var stockData = new List<StockViewModel>
            {
                new StockViewModel { ProductId = "1", Name = "Product A", Stock = 10 },
                new StockViewModel { ProductId = "2", Name = "Product B", Stock = 5 }
            };

            return View(stockData);
        }
    }
}