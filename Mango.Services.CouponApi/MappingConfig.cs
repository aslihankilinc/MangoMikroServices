using AutoMapper;
using Mango.Services.CouponApi.Models;
using Mango.Services.CouponApi.Models.Dto;
using Microsoft.Extensions.Logging;

namespace Mango.Services.CouponApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var expression = new MapperConfigurationExpression();
            expression.CreateMap<CouponDto, Coupon>();
            expression.CreateMap<Coupon, CouponDto>();
            var config = new MapperConfiguration(expression);
            return config;
        }
    }
}
