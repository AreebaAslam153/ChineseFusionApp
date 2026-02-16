using Microsoft.AspNetCore.Mvc;
using ChineseFusionApp.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace ChineseFusionApp.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        private readonly FirebaseClient firebase =
            new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");

        // Get Cart From Session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey,
                JsonConvert.SerializeObject(cart));
        }

        // Add to Cart
        public IActionResult AddToCart(string productId, string name, double price, string imageUrl)
        {
            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
                existingItem.Quantity++;
            else
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = 1
                });

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Show Cart
        public IActionResult Index()
        {
            return View(GetCart());
        }

        // 🔥 Place Order → Save to Firebase
        public async Task<IActionResult> PlaceOrder()
        {
            var cart = GetCart();

            foreach (var item in cart)
            {
                await firebase
                    .Child("Orders")
                    .PostAsync(item);
            }

            SaveCart(new List<CartItem>()); // Clear cart

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
