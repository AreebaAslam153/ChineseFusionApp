using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using ChineseFusionApp.Models;

namespace ChineseFusionApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public CustomerController()
        {
            _firebaseClient = new FirebaseClient(
                "https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        public async Task<IActionResult> Index()
        {
            var users = await _firebaseClient
                .Child("Customers")
                .OnceAsync<Customer>();

            var customerList = users.Select(u => new Customer
            {
                Id = u.Key,
                Name = u.Object.Name,
                Phone = u.Object.Phone,
                Address = u.Object.Address,
                Email = u.Object.Email,
                CreatedAt = u.Object.CreatedAt
            }).ToList();

            return View(customerList);
        }
    }
}
