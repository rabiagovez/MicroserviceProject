using ECommerceCase.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerceCase.UI.Controllers
{
    public class StockController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StockController> _logger;
        private static List<StockViewModel> _stocks = new List<StockViewModel>();

        public StockController(ILogger<StockController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            // Initialize with some sample data if empty
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5002/api/Stock");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<StockViewModel>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddStockViewModel model)
        {
            model.ProductId = Guid.NewGuid().ToString();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("http://localhost:5002/api/Stock", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                // Create notification for new stock
                var notificationData = new
                {
                    BuyerId = "SYSTEM",
                    OrderId = Guid.NewGuid(),
                    Message = $"Yeni stok eklendi: {model.Name} ({model.Stock} adet)",
                    CreatedAt = DateTime.UtcNow
                };
                
                var notificationJson = JsonConvert.SerializeObject(notificationData);
                var notificationContent = new StringContent(notificationJson, Encoding.UTF8, "application/json");
                await client.PostAsync("http://localhost:5003/api/Notification", notificationContent);
                
                return RedirectToAction("Index", "Stock");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"http://localhost:5002/api/Stock/{id}");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateStockViewModel>(jsonData);
                return View(values);
            }

            return RedirectToAction("Index", "Stock", new { error = "notfound" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateStockViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            var responseMessage = await client.PutAsync($"http://localhost:5002/api/Stock", stringContent);
            
            if (responseMessage.IsSuccessStatusCode)
            {
                // Create notification for stock update
                var notificationData = new
                {
                    BuyerId = "SYSTEM",
                    OrderId = Guid.NewGuid(),
                    Message = $"Stok güncellendi: {model.Name} ({model.Stock} adet)",
                    CreatedAt = DateTime.UtcNow
                };
                
                var notificationJson = JsonConvert.SerializeObject(notificationData);
                var notificationContent = new StringContent(notificationJson, Encoding.UTF8, "application/json");
                await client.PostAsync("http://localhost:5003/api/Notification", notificationContent);
                
                return RedirectToAction("Index", "Stock", new { success = "updated" });
            }
            
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            return RedirectToAction("Update", "Stock", new { id = model.ProductId, error = "update", message = errorContent });
        }

        
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"http://localhost:5002/api/Stock/{id}");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Stock");
            }

            return View();
        }
    }
}