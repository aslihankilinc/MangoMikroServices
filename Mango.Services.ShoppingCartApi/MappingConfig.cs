using AutoMapper;
using Mango.Services.CartApi.Models;
using Mango.Services.CartApi.Models.Dto;
namespace Mango.Services.CartApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var expression = new MapperConfigurationExpression();
            expression.CreateMap<CartHeaderDto, CartHeader>();
            expression.CreateMap<CartHeader, CartHeaderDto>();
            expression.CreateMap<CartDetailsDto, CartDetails>();
            expression.CreateMap<CartDetails, CartDetailsDto>();
            var config = new MapperConfiguration(expression);
            return config;
        }
    }
}
