using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Cart;
using Mango.Web.UI.Models.Dto.Order;
using Mango.Web.UI.Utility;

namespace Mango.Web.UI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }
    

        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = cartDto,
                Url = StaticBase.OrderApiBase + "/api/order/CreateOrder"
            });
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = stripeRequestDto,
                Url = StaticBase.OrderApiBase + "/api/order/CreateStripeSession"
            });
        }

        public async Task<ResponseDto?> GetAllOrder(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.OrderApiBase + "/api/order/GetOrders?userId=" + userId
            });
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.GET,
                Url = StaticBase.OrderApiBase + "/api/order/GetOrder/" + orderId
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = newStatus,
                Url = StaticBase.OrderApiBase + "/api/order/UpdateOrderStatus/" + orderId
            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = orderHeaderId,
                Url = StaticBase.OrderApiBase + "/api/order/ValidateStripeSession"
            });
        }
    }
}
