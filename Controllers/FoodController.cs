using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace ChineseFusionApp.Controllers
{
    public class FoodController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public FoodController()
        {
            _firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        // ✅ Show all food items
        public async Task<IActionResult> Index()
        {
            var foodItems = await _firebaseClient
                .Child("Menu")
                .OnceAsync<FoodItem>();

            var foodList = foodItems.Select(f => new FoodItem
            {
                Id = f.Key,
                Name = f.Object.Name,
                Category = f.Object.Category,
                Price = f.Object.Price
            }).ToList();

            return View(foodList);
        }

        // ✅ Show form to add a new item
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // ✅ Add new food item
        [HttpPost]
        public async Task<IActionResult> Add(FoodItem food)
        {
            await _firebaseClient
                .Child("Menu")
                .PostAsync(food);

            return RedirectToAction("Index");
        }

        // ✅ Delete item
        public async Task<IActionResult> Delete(string id)
        {
            await _firebaseClient
                .Child("Menu")
                .Child(id)
                .DeleteAsync();

            return RedirectToAction("Index");
        }
    }

    // ✅ Model
    public class FoodItem
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
    }
}
