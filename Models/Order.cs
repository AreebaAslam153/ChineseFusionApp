namespace ChineseFusionApp.Models
{
    public class Order
    {
        public string? Id { get; set; }
        public string CustomerId { get; set; }
        public List<CartItem> Items { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string CreatedAt { get; set; }
    }
}
