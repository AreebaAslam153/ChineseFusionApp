using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace ChineseFusionApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public OrderController()
        {
            _firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        // ✅ Display all orders
        public async Task<IActionResult> Index()
        {
            var orders = await _firebaseClient
                .Child("Orders")
                .OnceAsync<Order>();

            var orderList = orders.Select(o => new Order
            {
                Id = o.Key,
                CustomerName = o.Object.CustomerName,
                FoodItem = o.Object.FoodItem,
                Quantity = o.Object.Quantity,
                TotalPrice = o.Object.TotalPrice,
                Status = o.Object.Status,
                OrderDate = o.Object.OrderDate
            }).ToList();

            return View(orderList);
        }

        // ✅ Update order status (optional feature)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string id, string status)
        {
            await _firebaseClient
                .Child("Orders")
                .Child(id)
                .PatchAsync(new { Status = status });

            return RedirectToAction("Index");
        }
    }

    // ✅ Model
    public class Order
    {
        public string? Id { get; set; }
        public string? CustomerName { get; set; }
        public string? FoodItem { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string? Status { get; set; }
        public string? OrderDate { get; set; }
    }
}
