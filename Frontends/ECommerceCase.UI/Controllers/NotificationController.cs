using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;
using Newtonsoft.Json;

namespace ECommerceCase.UI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5003/api/Notification");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var notifications = JsonConvert.DeserializeObject<List<NotificationViewModel>>(jsonData);
                return View(notifications ?? new List<NotificationViewModel>());
            }

            return View(new List<NotificationViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5003/api/Notification/unread");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var count = JsonConvert.DeserializeObject<int>(jsonData);
                return Json(new { count });
            }

            return Json(new { count = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"http://localhost:5003/api/Notification/{id}/read", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
} 