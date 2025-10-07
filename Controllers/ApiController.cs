using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ChineseFusionApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly FirebaseAuth _auth;

        public ApiController(FirebaseAuth auth)
        {
            _auth = auth;
        }

        // 🔒 Protected route
        [HttpGet("protected")]
        public async Task<IActionResult> Protected()
        {
            // Read Authorization header: "Bearer <idToken>"
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
                return Unauthorized("Missing Authorization header");

            var token = authHeader.Replace("Bearer ", "");
            try
            {
                var decoded = await _auth.VerifyIdTokenAsync(token);
                var uid = decoded.Uid;

                // ✅ Successfully verified
                return Ok(new { message = "Token valid", uid });
            }
            catch (Exception ex)
            {
                return Unauthorized("Invalid token: " + ex.Message);
            }
        }
    }
}
