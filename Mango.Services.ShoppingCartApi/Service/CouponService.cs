using Mango.Services.CartApi.IContract;
using Mango.Services.CartApi.Models.Dto;
using Newtonsoft.Json;

namespace Mango.Services.CartApi.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClient;

        public CouponService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client=await _httpClient.CreateClient("Coupon")
                             .GetAsync($"/api/Coupon/GetByCode/{couponCode}");
            var apiContent = await client.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            return new CouponDto();
        }
    }
}
