using Mango.Services.CartApi.IContract;
using Mango.Services.CartApi.Models.Dto;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net.Http;

namespace Mango.Services.CartApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClient;
        public ProductService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductDto>> GetProducts()
        {
           
                var client = _httpClient.CreateClient("Product");
                var request = await client.GetAsync($"/api/product/getProductList");
                var apiContent = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                if (response.IsSuccess)
                {

                    return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                }
           
            return new List<ProductDto>();
        }
    }
}
