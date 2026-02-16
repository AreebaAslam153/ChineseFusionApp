using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using ChineseFusionApp.Models;

namespace ChineseFusionApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly IWebHostEnvironment _env;

        public CategoryController(IWebHostEnvironment env)
        {
            _env = env;
            _firebaseClient = new FirebaseClient(
                "https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }
        // 🔹 ADD CATEGORY (GET)
        public IActionResult Add()
        {
            return View();
        }

        // 🔹 ADD CATEGORY (POST)
        [HttpPost]
        public async Task<IActionResult> Add(Category cat, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                cat.ImageUrl = "/uploads/" + fileName;
            }

            await _firebaseClient.Child("Categories").PostAsync(cat);
            return RedirectToAction("Index", "Food");
        }

        // 🔹 EDIT (GET)
        public async Task<IActionResult> Edit(string id)
        {
            var cat = await _firebaseClient.Child("Categories").Child(id)
                .OnceSingleAsync<Category>();

            cat.Id = id;
            return View(cat);
        }

        // 🔹 EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Category cat, IFormFile imageFile)
        {
            // Fetch the existing category from Firebase
            var existingCat = await _firebaseClient
                .Child("Categories")
                .Child(cat.Id)
                .OnceSingleAsync<Category>();

            // If a new image is uploaded, replace it
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                cat.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                // No new image → keep existing image
                cat.ImageUrl = existingCat.ImageUrl;
            }

            await _firebaseClient
                .Child("Categories")
                .Child(cat.Id)
                .PutAsync(cat);

            return RedirectToAction("Index", "Food");
        }


        // 🔹 DELETE
        public async Task<IActionResult> Delete(string id)
        {
            await _firebaseClient.Child("Categories").Child(id).DeleteAsync();
            return RedirectToAction("Index", "Food");
        }
    }
}
