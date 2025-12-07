using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Mango.Web.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService; 
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await CartLoggedInUser());
        }

        private async Task<CartDto> CartLoggedInUser()
        {
            //oturum kullanıcısının id'sini alma
            var userId=User.Claims.Where(u=>u.Type==JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            var response=await _cartService.GetCartByUserIdAsnyc(userId);
            if(response is not null && response.IsSuccess)
            {
                CartDto cartDto = System.Text.Json.JsonSerializer.Deserialize<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
