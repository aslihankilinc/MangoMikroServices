using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Models.Dto.Auth;
using Mango.Web.UI.Utility;

namespace Mango.Web.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = registrationRequestDto,
                Url = StaticBase.AuthApiBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = loginRequestDto,
                Url = StaticBase.AuthApiBase + "/api/auth/login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = registrationRequestDto,
                Url = StaticBase.AuthApiBase + "/api/auth/register"
            });
        }
    }
}
