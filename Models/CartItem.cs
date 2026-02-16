namespace ChineseFusionApp.Models
{
    public class CartItem
    {
        public string ProductId { get; set; } = "";
        public string Name { get; set; } = "";
        public double Price { get; set; } = 0.0;
        public string ImageUrl { get; set; } = "";
        public int Quantity { get; set; } = 0;
    }
}
