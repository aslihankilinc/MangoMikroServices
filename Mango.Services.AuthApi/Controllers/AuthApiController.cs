using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        [HttpGet("register")]
        public IActionResult Register()
        {
            return Ok(new { status = "Auth API is running." });
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok(new { status = "Login endpoint hit." });
        }
    }
}
