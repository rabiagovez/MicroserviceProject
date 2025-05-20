using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrderFrontend.Models;

namespace OrderFrontend.Controllers
{
    public class OrderFrontendController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _orderServiceUrl;

        public OrderFrontendController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _orderServiceUrl = configuration["ServiceUrls:OrderService"];
        }

        // Sipariş oluşturma sayfasını göster
        public IActionResult Create()
        {
            var model = new CreateOrderViewModel();
            return View(model);
        }

        // Sipariş listesi sayfası
        public IActionResult Index()
        {
            // Gerçek uygulamada burası sipariş listesini API'den çeker
            return View();
        }

        // Sipariş oluşturma işlemi
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // API'ye gönderilecek komutu oluştur
                var createOrderCommand = new
                {
                    BuyerId = model.BuyerId,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Items = model.Items.Select(item => new
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity
                    }).ToList()
                };

                // API isteği gönder
                var content = new StringContent(
                    JsonSerializer.Serialize(createOrderCommand),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_orderServiceUrl}/api/Order", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var orderResponse = JsonSerializer.Deserialize<OrderResponseViewModel>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Başarılı sipariş oluşturma sonrası detay sayfasına yönlendir
                    return RedirectToAction("Details", new { id = orderResponse.OrderId });
                }

                ModelState.AddModelError("", "Sipariş oluşturulurken bir hata oluştu.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                return View(model);
            }
        }

        // Sipariş detay sayfası
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                // Gerçek uygulamada burada sipariş detayları API'den çekilir
                // Şimdilik sadece ID'yi görüntüleyelim
                var model = new OrderDetailsViewModel
                {
                    OrderId = id,
                    OrderDate = DateTime.UtcNow
                    // Diğer detaylar API'den alınacak
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Sipariş öğesi ekleme - AJAX çağrısı için
        [HttpPost]
        public IActionResult AddOrderItem()
        {
            return PartialView("_OrderItemPartial", new OrderItemViewModel());
        }
    }
}