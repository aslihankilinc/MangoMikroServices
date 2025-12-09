using AutoMapper;
using Azure;
using Mango.MessageBus.IContract;
using Mango.Services.CartApi.Data;
using Mango.Services.CartApi.IContract;
using Mango.Services.CartApi.Models;
using Mango.Services.CartApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mango.Services.CartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper mapper;
        private IProductService _productService;
        private ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private IConfiguration _configuration;
        public CartAPIController(AppDbContext db, IMapper mapper,IProductService productService,
            ICouponService couponService, IMessageBus messageBus, IConfiguration configuration)
        {
            _db = db;
            this.mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _configuration = configuration;
            _messageBus = messageBus;
        }

        //CartHeader → sepetin genel bilgisi (hangi kullanıcıya ait, ne zaman oluşturuldu, toplam fiyat vs.)
        // CartDetails → o sepetteki ürünlerin tek tek detayları(ürün ID, miktar vs.)
        //Yeni bir sepet oluştur ve o sepete bir ürün ekle
        [HttpPost("upsertCart")]
        public async Task<ResponseDto> UpsertCart(CartDto cartDto)
        {
            ResponseDto response = new();
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                response.Result = cartDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        [HttpPost("removeCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            ResponseDto response = new();
            try
            {
                // 1Silinmek istenen ürün satırını (CartDetails) bul
                // Eğer kayıt yoksa hata fırlatır (First yerine FirstOrDefault daha güvenli olur)
                CartDetails cartDetails = _db.CartDetails
                   .First(u => u.CartDetailsId == cartDetailsId);
                if (cartDetails == null)
                {
                    response.Message = "Kart  Bulunamadı";
                    response.IsSuccess = false;
                    return response;
                }
                //Aynı sepette (CartHeader) kaç ürün olduğunu say
                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                //Ürün satırını (CartDetails) sil
                _db.CartDetails.Remove(cartDetails);
                //Eğer silinen son ürün ise sepeti (CartHeader) de sil
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                response.Result = true;
            }
            catch (Exception ex)
            {
               response.Message = ex.Message.ToString();
               response.IsSuccess = false;
            }
            return response;
        }


        [HttpGet("getCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            ResponseDto response = new();
            try
            {
                CartDto cart = new()
                {
                    CartHeader = mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));
                var productList = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productList.FirstOrDefault(p => p.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                //kupon var mı
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                  var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                response.Result = cart;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
               response.Message = ex.Message;
            }
            return response;
        }

        //kuponu uygula

        [HttpPost("applyCart")]
        public async Task<object> ApplyCart([FromBody] CartDto cartDto)
        {
            ResponseDto response = new();
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                
            }
            return response;
        }

        [HttpPost("sendEmail")]

        public async Task<ResponseDto> SendEmail([FromBody] CartDto cartDto)
        {
            ResponseDto response = new();
            try
            {
                await _messageBus.PublishMessage(cartDto, 
                    _configuration.GetValue<string>("TopicAndQueueNames:EmailQueue"));
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }
            return response;
        }

    }
}
