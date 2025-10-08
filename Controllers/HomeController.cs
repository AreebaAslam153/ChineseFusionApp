using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;

namespace ChineseFusionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public HomeController()
        {
            _firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Fetch counts from Firebase
                var orders = await _firebaseClient.Child("Orders").OnceAsync<object>();
                var customers = await _firebaseClient.Child("Users").OnceAsync<object>();
                var menuItems = await _firebaseClient.Child("Menu").OnceAsync<object>();
                var transactions = await _firebaseClient.Child("Transactions").OnceAsync<object>();

                // Calculate Pending Payments
                int pendingPayments = 0;
                foreach (var t in transactions)
                {
                    var data = t.Object as dynamic;
                    if (data != null && data?.Status == "Pending")
                        pendingPayments++;
                }

                // Pass data to view
                ViewBag.TotalOrders = orders.Count;
                ViewBag.TotalCustomers = customers.Count;
                ViewBag.TotalMenuItems = menuItems.Count;
                ViewBag.PendingPayments = pendingPayments;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"⚠️ Error fetching data: {ex.Message}";
            }

            return View();
        }
    }
}
