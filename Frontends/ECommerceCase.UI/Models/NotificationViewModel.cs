namespace ECommerceCase.UI.Models
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string BuyerId { get; set; }
        public Guid OrderId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 