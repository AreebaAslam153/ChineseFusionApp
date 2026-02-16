namespace ChineseFusionApp.Models
{
    public class Customer
    {
        public string? Id { get; set; }

        // Signup fields (from Android)
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        // Metadata
        public string? CreatedAt { get; set; }
    }
}
