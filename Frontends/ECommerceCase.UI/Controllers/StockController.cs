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
            if (!_stocks.Any())
            {
               
            }

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
                return RedirectToAction("Index", "Stock");
            }

            return View();
        }
        
        // public async Task<IActionResult> Edit(int id)
        // {
        //     try
        //     {
        //         var stock = _stocks.FirstOrDefault(s => s.Id == id);
        //         if (stock == null)
        //         {
        //             TempData["ErrorMessage"] = "Stock not found.";
        //             return RedirectToAction(nameof(Index));
        //         }
        //
        //         return View(stock);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while fetching stock for edit");
        //         TempData["ErrorMessage"] = "An error occurred while fetching stock details.";
        //         return RedirectToAction(nameof(Index));
        //     }
        // }

        [HttpPost]
        public async Task<IActionResult> Edit(StockViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var existingStock = _stocks.FirstOrDefault(s => s.Id == model.Id);
                if (existingStock == null)
                {
                    TempData["ErrorMessage"] = "Stock not found.";
                    return RedirectToAction(nameof(Index));
                }

                existingStock.ProductName = model.ProductName;
                existingStock.Quantity = model.Quantity;
                existingStock.LastUpdated = DateTime.Now;

                TempData["SuccessMessage"] = "Stock updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock");
                TempData["ErrorMessage"] = "An error occurred while updating stock.";
                return View(model);
            }
        }

        [HttpPost]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     try
        //     {
        //         var stock = _stocks.FirstOrDefault(s => s.Id == id);
        //         if (stock == null)
        //         {
        //             TempData["ErrorMessage"] = "Stock not found.";
        //             return RedirectToAction(nameof(Index));
        //         }
        //
        //         _stocks.Remove(stock);
        //         TempData["SuccessMessage"] = "Stock deleted successfully.";
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while deleting stock");
        //         TempData["ErrorMessage"] = "An error occurred while deleting stock.";
        //         return RedirectToAction(nameof(Index));
        //     }
        // }
        
        public async Task<IActionResult> DeleteCategory(Guid id)

        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("http://localhost:5002/api/Stock/" + id);

            if (responseMessage.IsSuccessStatusCode)

                return RedirectToAction("Index", "Stock");

            return View();

        }
    }
}