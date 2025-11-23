using Mango.Web.UI.IContract;
using Mango.Web.UI.Models.Dto;
using Mango.Web.UI.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Mango.Web.UI.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool isBerarer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "text/plain");
                //token
                //alt servislerde authorization erisimi
                //Token Bearer olarak tanımlandı
                if (isBerarer)
                {
                    var token = _tokenService.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");

                }

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                message.Method = requestDto.ApiType switch
                {
                    EnumApiType.POST => HttpMethod.Post,
                    EnumApiType.DELETE => HttpMethod.Delete,
                    EnumApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get
                };

                apiResponse = await client.SendAsync(message);

                var errorMessages = new Dictionary<HttpStatusCode, string>
                 {
                     { HttpStatusCode.NotFound, "Not Found" },
                     { HttpStatusCode.Forbidden, "Access Denied" },
                     { HttpStatusCode.Unauthorized, "Unauthorized" },
                     { HttpStatusCode.InternalServerError, "Internal Server Error" }
                 };

                if (errorMessages.TryGetValue(apiResponse.StatusCode, out var msg))
                {
                    return new ResponseDto { IsSuccess = false, Message = msg };
                }

                var content = await apiResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseDto>(content);
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
