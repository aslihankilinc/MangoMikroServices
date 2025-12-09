using Mango.Services.OrderApi.Models.Dto.Product;

namespace Mango.Services.OrderApi.IContract
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
