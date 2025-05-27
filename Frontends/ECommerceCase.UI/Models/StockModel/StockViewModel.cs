using System;

namespace ECommerceCase.UI.Models
{
    public class StockViewModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}