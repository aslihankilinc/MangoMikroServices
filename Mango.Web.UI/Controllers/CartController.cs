using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

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


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Kupon  uygulandı";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private async Task<CartDto> CartLoggedInUser()
        {
            //oturum kullanıcısının id'sini alma
            var userId=User.Claims.Where(u=>u.Type==JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            var response=await _cartService.GetCartByUserIdAsnyc(userId);
            if(response is not null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(CartDto cartDto)
        {
            var cart =await CartLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault().Value;
            ResponseDto? response = await _cartService.EmailCart(cart);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Mail gönderildi";
                return RedirectToAction(nameof(Index));
            }
            return View();


        }
        }
}
