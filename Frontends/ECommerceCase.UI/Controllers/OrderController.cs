using System.Text;
using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;
using Newtonsoft.Json;
using OrderService.Application.Models;

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
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5001/api/Order");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {JsonData}", jsonData);

                // DTO'ya deserialize et
                var orderDtos = JsonConvert.DeserializeObject<List<OrderDto>>(jsonData);
                _logger.LogInformation("Deserialized Orders Count: {Count}", orderDtos?.Count ?? 0);
                if (orderDtos != null)
                {
                    foreach (var dto in orderDtos)
                    {
                        _logger.LogInformation("Order: Id={OrderId}, Email={Email}, ProductName={ProductName}", 
                            dto.OrderId, dto.Email, dto.ProductName);
                    }
                }

                // ViewModel'e dönüştür
                var groupedOrders = orderDtos
                    .GroupBy(dto => dto.OrderId)
                    .Select(group => new ECommerceCase.UI.Models.OrderViewModel
                    {
                        OrderId = group.Key,
                        BuyerId = group.First().BuyerId,
                        Email = group.First().Email,
                        Items = group.Select(item => new ECommerceCase.UI.Models.OrderItemViewModel
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity
                        }).ToList()
                    }).ToList();


                return View(groupedOrders);
            }

            return View(new List<OrderViewModel>()); // boş liste döndür, hata kontrolü açısından güvenli
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch stocks from Stock API
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5002/api/Stock");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var stocks = JsonConvert.DeserializeObject<List<StockViewModel>>(jsonData);
                ViewBag.Stocks = stocks;
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
            model.BuyerId = Guid.NewGuid().ToString();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("http://localhost:5001/api/Order", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Order");
            }
            return View();
        }
    }
}