using Microsoft.AspNetCore.Mvc;

namespace ECommerceCase.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
