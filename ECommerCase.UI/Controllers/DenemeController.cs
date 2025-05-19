using Microsoft.AspNetCore.Mvc;

namespace ECommerCase.UI.Controllers
{
    public class DenemeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
