using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;

namespace ChineseFusionApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public TransactionController()
        {
            //_firebaseClient = new FirebaseClient("https://chinesefusionapp.firebaseio.com/");
            _firebaseClient = new FirebaseClient("https://chinesefusionapp-default-rtdb.firebaseio.com/");
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _firebaseClient
                .Child("Transactions")
                .OnceAsync<dynamic>();

            var transactionList = transactions.Select(t => new
            {
                TransactionId = t.Object.TransactionId,
                CustomerName = t.Object.CustomerName,
                OrderId = t.Object.OrderId,
                Amount = t.Object.Amount,
                Status = t.Object.Status,
                DateTime = t.Object.DateTime
            }).ToList();

            ViewBag.Transactions = transactionList;

            return View();
        }
    }
}
