using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly FirebaseAuth _firebaseAuth;

    public AccountController(FirebaseAuth firebaseAuth)
    {
        _firebaseAuth = firebaseAuth;
    }

    [HttpPost]
    public async Task<IActionResult> VerifyToken([FromBody] string idToken)
    {
        try
        {
            var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);
            var uid = decodedToken.Uid;
            return Ok($"Verified user: {uid}");
        }
        catch (Exception ex)
        {
            return BadRequest("Invalid token: " + ex.Message);
        }
    }
}
