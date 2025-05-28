namespace OrderService.Application.Models;

public class OrderDto
{
    public Guid OrderId { get; set; } // bu alan şart
    public string ProductId { get; set; }
    public string BuyerId { get; set; }
    public string Email { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
