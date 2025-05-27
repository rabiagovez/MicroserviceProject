using System.ComponentModel.DataAnnotations;

namespace ECommerceCase.UI.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string EventType { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        
        public int TotalQuantity => Items?.Sum(item => item.Quantity) ?? 0;
        
        public int TotalOrders { get; set; }

        public int BuyerId { get; set; }
    }

    public class OrderItemViewModel
    {
   
        public string ProductId { get; set; } = string.Empty;
        
        public string ProductName { get; set; } = string.Empty;
        
        public int Quantity { get; set; }
    }
} 