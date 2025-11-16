using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthApi.IContract
{
    public class IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
