namespace StockService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();        
        public string ProductId { get; set; } 
        public string Name { get; set; }    
        public int Stock { get; set; }                        
    }
}
