using Mango.Web.UI.Models.Dto.Product;
using Mango.Web.UI.CartApi.Models.Dto;

namespace Mango.Web.UI.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
