using Mango.Services.EmailApi.Models.Dto;

namespace Mango.Services.EmailApi.IContract
{
    public interface IEmailService
    {
        Task EmailLog(CartDto cartDto);
    }
}
