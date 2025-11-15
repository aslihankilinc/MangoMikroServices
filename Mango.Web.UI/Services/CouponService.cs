using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Utility;

namespace Mango.Web.UI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = couponDto,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI"
            });
        }

        public async Task<ResponseDto?> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.DELETE,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.PUT,
                Data = couponDto,
                Url = StaticBase.CouponApiBase + "/api/CouponAPI"
            });
        }
    }
}
