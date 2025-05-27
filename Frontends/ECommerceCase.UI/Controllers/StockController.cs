using ECommerceCase.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCase.UI.Controllers
{
    public class StockController : Controller
    {
        private readonly ILogger<StockController> _logger;
        private static List<StockViewModel> _stocks = new List<StockViewModel>();

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
            // Initialize with some sample data if empty
            if (!_stocks.Any())
            {
                _stocks.Add(new StockViewModel { Id = 1, ProductName = "Test Product 1", Quantity = 100, LastUpdated = DateTime.Now });
                _stocks.Add(new StockViewModel { Id = 2, ProductName = "Test Product 2", Quantity = 50, LastUpdated = DateTime.Now });
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(_stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stocks");
                TempData["ErrorMessage"] = "An error occurred while fetching stocks.";
                return View(new List<StockViewModel>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StockViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.Id = _stocks.Count > 0 ? _stocks.Max(s => s.Id) + 1 : 1;
                model.LastUpdated = DateTime.Now;
                _stocks.Add(model);

                TempData["SuccessMessage"] = "Stock created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating stock");
                TempData["ErrorMessage"] = "An error occurred while creating stock.";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var stock = _stocks.FirstOrDefault(s => s.Id == id);
                if (stock == null)
                {
                    TempData["ErrorMessage"] = "Stock not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock for edit");
                TempData["ErrorMessage"] = "An error occurred while fetching stock details.";
                return RedirectToAction(nameof(Index));
            }
        }

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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var stock = _stocks.FirstOrDefault(s => s.Id == id);
                if (stock == null)
                {
                    TempData["ErrorMessage"] = "Stock not found.";
                    return RedirectToAction(nameof(Index));
                }

                _stocks.Remove(stock);
                TempData["SuccessMessage"] = "Stock deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting stock");
                TempData["ErrorMessage"] = "An error occurred while deleting stock.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}