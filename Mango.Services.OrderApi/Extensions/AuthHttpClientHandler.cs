using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mango.Services.OrderApi.Extensions
{
    //DelegatingHandler HttpClient istekleri gönderilmeden önce veya
    //cevap geldikten sonra araya girip işlem yapmamızı
    //sağlayan özel bir “pipeline” sınıfıdır.
    //HttpClient program.cs yapılandırmalarına eklenecek
    public class AuthHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accesToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
