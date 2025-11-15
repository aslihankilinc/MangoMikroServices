using Mango.Web.UI.Utility;
using System.Net.Mime;
using System.Security.AccessControl;

namespace Mango.Web.UI.Models.Dto
{
    public class RequestDto
    {
        public EnumApiType ApiType { get; set; } = EnumApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
        // public ContentType ContentType { get; set; } = ContentType.Json;

    }
}
