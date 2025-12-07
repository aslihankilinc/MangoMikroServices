using Mango.Services.CartApi.IContract;
using Mango.Services.CartApi.Models.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

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
            var client =  _httpClient.CreateClient("Coupon");
            var request = await client.GetAsync($"api/coupon/getByCode/{couponCode}");
            var apiContent = await request.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            return new CouponDto();
        }
    }
}
