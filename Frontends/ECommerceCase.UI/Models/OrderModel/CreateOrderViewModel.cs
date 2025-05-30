using System;
using System.Collections.Generic;

namespace ECommerceCase.UI.Models
{
    public class CreateOrderViewModel
    {
        public string BuyerId { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }
}

public class OrderItemViewModel
{
    public string ProductId { get; set; }
    
    public string ProductName { get; set; }
    
    public int Quantity { get; set; }
}
