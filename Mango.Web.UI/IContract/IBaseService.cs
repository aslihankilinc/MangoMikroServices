using Mango.Web.UI.Models.Dto;

namespace Mango.Web.UI.IContract
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto,bool isBearer=true);
    }
}
