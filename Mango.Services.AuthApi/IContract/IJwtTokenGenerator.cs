using Mango.Services.AuthApi.Models;

namespace Mango.Services.AuthApi.IContract
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
