using System.ComponentModel.DataAnnotations;

namespace ECommerceCase.UI.Models
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public string Email { get; set; }
        public int TotalQuantity => Items?.Sum(item => item.Quantity) ?? 0;
        
        public int TotalOrders { get; set; }

        public string BuyerId { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductId { get; set; }
        
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
    }
} 