using AutoMapper;
using Mango.Services.ProductApi.Models;
using Mango.Services.ProductApi.Models.Dto;

namespace Mango.Services.ProductApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var expression = new MapperConfigurationExpression();
            expression.CreateMap<ProductDto, Product>();
            expression.CreateMap<Product, ProductDto>();
            var config = new MapperConfiguration(expression);
            return config;
        }
    }
}
