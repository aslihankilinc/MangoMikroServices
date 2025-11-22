using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Auth;
using Mango.Web.UI.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.UI.Controllers
{
    public class AuthController(IAuthService _authService, ITokenService _tokenService) : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                SigInUser(loginResponseDto);
                _tokenService.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=Const.Admin,Value=Const.Admin},
                new SelectListItem{Text=Const.Customer,Value=Const.Customer},
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = Const.Customer;
                }
                assingRole = await _authService.AssignRoleAsync(obj);
                if (assingRole != null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Kayıt Başarılı";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=Const.Admin,Value=Const.Admin},
                new SelectListItem{Text=Const.Customer,Value=Const.Customer},
            };

            ViewBag.RoleList = roleList;
            return View(obj);
        }


        //cookie tabanlı oturum açmasılması
        private async Task SigInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            //Token decode ediliyor
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            //Roller ekleniyor
            foreach (var role in jwt.Claims.Where(u => u.Type == "role").Select(u => u.Value))
            {
                identity.AddClaim(new Claim("role", role));
            }

            //Kullanıcı “principal” oluşturuluyor
            //ClaimsPrincipal, kimlik bilgilerini (identity) ve claim’leri taşıyan nesnedir
            var principal = new ClaimsPrincipal(identity);
            //Oturum açma işlemi gerçekleştiriliyor
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenService.RemoveToken();
            return RedirectToAction("Index", "Home");
        }

    }
}
