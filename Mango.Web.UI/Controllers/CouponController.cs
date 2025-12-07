using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Coupon;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Mango.Web.UI.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService) => _couponService = couponService;

        public async Task<IActionResult> Index()
        {
            List<CouponDto>? list = new();

            ResponseDto? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        public async Task<IActionResult> CouponCreated(int couponId)
        {
            ResponseDto? response = new();
            if (couponId > 0)
                response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreated(CouponDto model)
        {
            
                ResponseDto? response;
                if (model.CouponId > 0)
                {
                    response = await _couponService.UpdateCouponsAsync(model);
                }
                else
                {
                    response = await _couponService.CreateCouponsAsync(model);
                }
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Kupon Oluşturuldu";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
        
            return View(model);
        }
        public async Task<IActionResult> CouponDeleted(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDeleted(CouponDto model)
        {
            ResponseDto? response = await _couponService.DeleteCouponsAsync(model.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Kupon Silindi";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(model);
        }
    }
}
