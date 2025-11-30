using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Product;
using Mango.Web.UI.Utility;

namespace Mango.Web.UI.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = productDto,
                Url = StaticBase.ProductApiBase + "/api/product/created",
                ContentType = EnumContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.DELETE,
                Url = StaticBase.ProductApiBase + "/api/product/deleted/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.ProductApiBase + "/api/product/getProductList"
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.ProductApiBase + "/api/product/getProduct/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = productDto,
                Url = StaticBase.ProductApiBase + "/api/product/created",
                ContentType = EnumContentType.MultipartFormData
            });
        }
    }
}
