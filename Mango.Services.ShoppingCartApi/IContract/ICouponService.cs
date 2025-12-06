using Mango.Services.CartApi.Models.Dto;

namespace Mango.Services.CartApi.IContract
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
