using Microsoft.AspNetCore.Mvc;

namespace ECommerCase.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
