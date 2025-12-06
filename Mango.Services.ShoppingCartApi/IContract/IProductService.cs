using Mango.Services.CartApi.Models.Dto;

namespace Mango.Services.CartApi.IContract
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
    }
}
