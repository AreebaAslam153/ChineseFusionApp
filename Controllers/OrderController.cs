using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using ChineseFusionApp.Models;

namespace ChineseFusionApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public OrderController()
        {
            _firebaseClient = new FirebaseClient(
                "https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        // 🔹 DISPLAY ORDERS (ADMIN)
        public async Task<IActionResult> Index()
        {
            var orders = await _firebaseClient.Child("Orders").OnceAsync<Order>();
            var customers = await _firebaseClient.Child("Customers").OnceAsync<Customer>();

            var customerMap = customers.ToDictionary(c => c.Key, c => c.Object);

            var result = orders.Select(o =>
            {
                customerMap.TryGetValue(o.Object.CustomerId, out var cust);

                return new OrderViewModel
                {
                    OrderId = o.Key,
                    CustomerName = cust?.Name ?? "Unknown",
                    CustomerEmail = cust?.Email ?? "N/A",
                    Items = o.Object.Items,
                    TotalAmount = o.Object.TotalAmount,
                    Status = o.Object.Status
                };
            }).ToList();

            return View(result);
        }

        // 🔹 UPDATE ORDER STATUS
        public async Task<IActionResult> UpdateStatus(string id, string status)
        {
            var order = await _firebaseClient.Child("Orders").Child(id).OnceSingleAsync<Order>();
            order.Status = status;

            await _firebaseClient.Child("Orders").Child(id).PutAsync(order);
            return RedirectToAction("Index");
        }
    }
}
