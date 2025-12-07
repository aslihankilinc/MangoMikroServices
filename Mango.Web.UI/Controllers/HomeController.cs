using Mango.Web.UI.IContract;
using Mango.Web.UI.Models;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Cart;
using Mango.Web.UI.Models.Dto.Product;
using Mango.Web.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using static Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
namespace Mango.Web.UI.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }


        public async Task<IActionResult> Index()
        {
            List<ProductDto>? list = new();

            ResponseDto? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? model = new();

            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("productDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.FindFirstValue(Sub)
                }
            };
            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };
            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
            cartDto.CartDetails = cartDetailsDtos;

            ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Ürün sepete eklendi";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            var result = await _productService.GetProductByIdAsync(productDto.ProductId);
            productDto = JsonConvert.DeserializeObject<ProductDto>(result.Result.ToString());
            return View(productDto);
        }




        //rol eklendi
        [Authorize(Roles = Const.Admin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
