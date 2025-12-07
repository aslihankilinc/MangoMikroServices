using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Cart;
using Mango.Web.UI.Utility;

namespace Mango.Web.UI.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = cartDto,
                Url = StaticBase.CartApiBase + "/api/cart/applyCart"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = cartDto,
                Url = StaticBase.CartApiBase + "/api/cart/emailCartRequest"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.CartApiBase + "/api/cart/getCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = cartDetailsId,
                Url = StaticBase.CartApiBase + "/api/cart/removeCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = cartDto,
                Url = StaticBase.CartApiBase + "/api/cart/upsertCart"
            });
        }
    }
}
