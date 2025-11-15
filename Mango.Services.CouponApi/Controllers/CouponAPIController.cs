using AutoMapper;
using Mango.Services.CouponApi.Data;
using Mango.Services.CouponApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mango.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private  IMapper _mapper;
        public CouponAPIController(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();   
        }

        [HttpGet]
        public ResponseDto GetCoupons()
        {
            try
            {
                var list = _db.Coupons.ToList();                
                _response.Result = _mapper.Map<List<CouponDto>>(list);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetCoupons(int id)
        {
            try
            {
                var item = _db.Coupons.FirstOrDefault(w=>w.CouponId==id);
                _response.Result = _mapper.Map<CouponDto>(item);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
