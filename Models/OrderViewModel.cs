namespace ChineseFusionApp.Models
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<CartItem> Items { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
