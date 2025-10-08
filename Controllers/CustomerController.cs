using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace ChineseFusionApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public CustomerController()
        {
            _firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        // ✅ Fetch and display all registered customers
        public async Task<IActionResult> Index()
        {
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<Customer>();

            var customerList = users.Select(u => new Customer
            {
                Id = u.Key,
                Name = u.Object.Name,
                Email = u.Object.Email,
                CreatedAt = u.Object.CreatedAt
            }).ToList();

            return View(customerList);
        }
    }

    // ✅ Model for customers
    public class Customer
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? CreatedAt { get; set; }
    }
}
