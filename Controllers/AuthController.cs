using FirebaseAdmin.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;


namespace ChineseFusionApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly FirebaseAuth _auth;

        public AuthController(FirebaseAuth auth)
        {
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            try
            {
                // ✅ Create user in Firebase Authentication
                var userArgs = new UserRecordArgs
                {
                    DisplayName = name,
                    Email = email,
                    Password = password,
                    EmailVerified = false
                   
                };

                var userRecord = await _auth.CreateUserAsync(userArgs);

                // ✅ Add this line here — your Firebase Realtime Database URL
                var firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");

                // ✅ Save user info to Realtime Database
                await firebaseClient
                    .Child("Users")
                    .Child(userRecord.Uid)
                    .PutAsync(new
                    {
                        Name=userRecord.DisplayName,
                        Email = userRecord.Email,
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    });

                ViewBag.Message = $"✅ User {userRecord.Email} registered successfully!";
                ViewBag.ShowLoginButton = true;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Error: {ex.Message}";
                ViewBag.ShowLoginButton = false;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            ViewBag.Message = $"Login attempted for {email}";
            HttpContext.Session.SetString("UserEmail", email);
            return RedirectToAction("Dashboard", "Home");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
