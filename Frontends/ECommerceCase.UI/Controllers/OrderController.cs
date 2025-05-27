using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;
using Newtonsoft.Json;

namespace ECommerceCase.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
             var client = _httpClientFactory.CreateClient();
             var responseMessage = await client.GetAsync("http://localhost:5001/api/Order");
             if (responseMessage.IsSuccessStatusCode)
             {
                 var jsonData = await responseMessage.Content.ReadAsStringAsync();
                 var values = JsonConvert.DeserializeObject<List<OrderViewModel>>(jsonData);
                 return View(values ?? new List<OrderViewModel>());
             }
                
            return View(new List<OrderViewModel>());
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch stock data
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5002/api/Stock");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var stocks = JsonConvert.DeserializeObject<List<StockViewModel>>(jsonData);
                ViewBag.Stocks = stocks ?? new List<StockViewModel>();
            }
            else
            {
                ViewBag.Stocks = new List<StockViewModel>();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Fetch stocks again for the view
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync("http://localhost:5002/api/Stock");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var stocks = JsonConvert.DeserializeObject<List<StockViewModel>>(jsonData);
                    ViewBag.Stocks = stocks ?? new List<StockViewModel>();
                }
                return View(model);
            }

            // TODO: API'ye sipariş oluşturma isteği gönder
            return RedirectToAction(nameof(Index));
        }
    }
}