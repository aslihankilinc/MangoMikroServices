using Mango.Services.CouponApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CouponAPIController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public object GetCoupons()
        {
            try
            {
                var list = _db.Coupons.ToList();
                return list;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        [HttpGet]
        [Route("{id:int}")]
        public object GetCoupons(int id)
        {
            try
            {
                var item = _db.Coupons.FirstOrDefault(w=>w.CouponId==id);
                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
