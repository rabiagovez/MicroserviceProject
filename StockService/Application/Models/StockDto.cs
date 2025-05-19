namespace StockService.Application.Models
{
    public class StockDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
