namespace ECommerceCase.UI.Models
{
    public class CreateOrderViewModel
    {
        public string BuyerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }
}