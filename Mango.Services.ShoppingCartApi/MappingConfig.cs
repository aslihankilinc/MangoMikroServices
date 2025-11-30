using AutoMapper;
namespace Mango.Services.ShoppingCartApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var expression = new MapperConfigurationExpression();
            var config = new MapperConfiguration(expression);
            return config;
        }
    }
}
