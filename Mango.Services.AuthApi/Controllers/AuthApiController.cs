using Mango.Services.AuthApi.IContract;
using Mango.Services.AuthApi.Models.Dto;
using Mango.Services.AuthApi.RabbitMQ.IContract;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        private ResponseDto _response;
        private IRabbitMQAuthMessageSender _rabbit;
        private IConfiguration _configuration;

        public AuthApiController(IAuthService authService,IRabbitMQAuthMessageSender rabbit, IConfiguration configuration)
        {
            _authService = authService;
            _response = new ResponseDto();
            _rabbit = rabbit;
            _configuration = configuration;
        }

        //kullanıcı oluştrma
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            var queueKey= _configuration.GetValue<string>("RabbitMQAuthSettings:QueueName");
            _rabbit.SendMessage(model, queueKey);


            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Kullanici adi ya da şifre hatası";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);

        }
//rol olusturma
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody]RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Olusturulamadı";
                return BadRequest(_response);
            }
            return Ok(_response);

        }
    }
}
