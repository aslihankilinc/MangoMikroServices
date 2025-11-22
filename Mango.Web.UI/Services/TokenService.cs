using Mango.Web.UI.IContract;
using Mango.Web.UI.Utility;
using Newtonsoft.Json.Linq;
namespace Mango.Web.UI.Services
{
    public class TokenService : ITokenService
    {
        //TODO: Implement Token Service Logic Using HttpContextAccessor
        //program.cs innjekte
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetToken()
        {
            bool hasToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(Const.TokenCookie, out var token);
            return hasToken is true ? token : null;
        }

        public void RemoveToken()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(Const.TokenCookie);
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(Const.TokenCookie, token);
        }
    }
}
