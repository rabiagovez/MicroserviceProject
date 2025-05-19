namespace StockService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();        
        public string ProductId { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty;      
        public int Stock { get; set; }                        
    }
}
