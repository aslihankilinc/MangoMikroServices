using Mango.Services.AuthApi.IContract;
using Mango.Services.AuthApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mango.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok(new { status = "Login endpoint hit." });
        }
    }
}
