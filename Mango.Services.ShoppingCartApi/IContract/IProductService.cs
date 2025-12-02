using Mango.Services.ShoppingCartApi.Models.Dto;

namespace Mango.Services.ShoppingCartApi.IContract
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
    }
}
