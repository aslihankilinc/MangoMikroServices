using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dto;
namespace Mango.Services.ShoppingCartApi
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
