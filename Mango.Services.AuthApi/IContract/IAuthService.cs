
using Mango.Services.AuthApi.Models.Dto;
namespace Mango.Services.AuthApi.IContract
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
