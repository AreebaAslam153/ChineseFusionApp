using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using ChineseFusionApp.Models;

namespace ChineseFusionApp.Controllers
{
    public class FoodController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly IWebHostEnvironment _env;

        public FoodController(IWebHostEnvironment env)
        {
            _env = env;
            _firebaseClient = new FirebaseClient(
                "https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }
        public IActionResult AddChoice()
        {
            return View();
        }


        // 🔹 INDEX
        public async Task<IActionResult> Index()
        {
            var data = await _firebaseClient.Child("Menu").OnceAsync<FoodItem>();

            var list = data.Select(x => new FoodItem
            {
                Id = x.Key,
                Name = x.Object.Name,
                Category = x.Object.Category,
                Price = x.Object.Price,
                ImageUrl = x.Object.ImageUrl
            }).ToList();

            ViewBag.Categories = await GetCategories();
            return View(list);
        }

        // 🔹 ADD (GET)
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await GetCategories();
            return View();
        }

        // 🔹 ADD (POST)
        [HttpPost]
        public async Task<IActionResult> Add(FoodItem food, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                food.ImageUrl = "/uploads/" + fileName;
            }

            await _firebaseClient.Child("Menu").PostAsync(food);
            return RedirectToAction("Index");
        }

        // 🔹 EDIT (GET)
        public async Task<IActionResult> Edit(string id)
        {
            var item = await _firebaseClient.Child("Menu").Child(id).OnceSingleAsync<FoodItem>();
            item.Id = id;

            ViewBag.Categories = await GetCategories();
            return View(item);
        }

        // 🔹 EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(FoodItem food, IFormFile imageFile)
        {
            // Fetch existing item from Firebase
            var existingFood = await _firebaseClient
                .Child("Menu")
                .Child(food.Id)
                .OnceSingleAsync<FoodItem>();

            // Only update image if a new file is uploaded
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                food.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                // Keep existing image
                food.ImageUrl = existingFood.ImageUrl;
            }

            await _firebaseClient
                .Child("Menu")
                .Child(food.Id)
                .PutAsync(food);

            return RedirectToAction("Index");
        }


        // 🔹 DELETE
        public async Task<IActionResult> Delete(string id)
        {
            await _firebaseClient.Child("Menu").Child(id).DeleteAsync();
            return RedirectToAction("Index");
        }

        // 🔹 CATEGORY HELPER
        private async Task<List<Category>> GetCategories()
        {
            var data = await _firebaseClient.Child("Categories").OnceAsync<Category>();

            return data.Select(c => new Category
            {
                Id = c.Key,
                Name = c.Object.Name,
                Description = c.Object.Description,
                ImageUrl = c.Object.ImageUrl
            }).ToList();
        }
        
    }
}
