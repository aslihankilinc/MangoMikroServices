using AutoMapper;
using Mango.Services.ShoppingCartApi.Data;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper mapper;
        public CartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            this.mapper = mapper;
        }

        //CartHeader → sepetin genel bilgisi (hangi kullanıcıya ait, ne zaman oluşturuldu, toplam fiyat vs.)
        // CartDetails → o sepetteki ürünlerin tek tek detayları(ürün ID, miktar vs.)
        //Yeni bir sepet oluştur ve o sepete bir ürün ekle
        [HttpPost("upsertCart")]
        public async Task<ResponseDto> UpserCart(CartDto cartDto)
        {
            ResponseDto response = new();
            try
            {
                var cartHeader = await _db.CartHeaders.AsNoTracking()
                                   .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeader is null)
                {
                    CartHeader cartHeader1 = mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();

                }
                else
                {
                    //Kullanıcı aynı ürünü tekrar sepete eklerse yeni satır ekleme
                    //sadece miktarı  artır
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                      u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                      u.CartHeaderId == cartHeader.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                        _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
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

                foreach (var item in cart.CartDetails)
                {
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
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

    }
}
