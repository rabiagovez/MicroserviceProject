using System;

namespace ECommerceCase.UI.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
