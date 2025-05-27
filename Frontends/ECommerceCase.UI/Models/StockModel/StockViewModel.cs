using System;

namespace ECommerceCase.UI.Models
{
    public class StockViewModel
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}