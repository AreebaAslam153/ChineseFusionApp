using Microsoft.AspNetCore.Mvc;

namespace ChineseFusionApp.Controllers
{
    public class PagesController : Controller
    {
        // GET: /Pages/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Pages/Login
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            if (Email == "admin@chinese.com" && Password == "123")
            {
                ViewBag.Message = "Login successful!";
            }
            else
            {
                ViewBag.Message = "Invalid email or password.";
            }
            return View();
        }

        // GET: /Pages/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Pages/Register
        [HttpPost]
        public IActionResult Register(string Name, string Email, string Password)
        {
            ViewBag.Message = "Registration successful for " + Email;
            return View();
        }
    }
}
