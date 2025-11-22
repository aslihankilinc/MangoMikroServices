using System.ComponentModel.DataAnnotations;

namespace Mango.Web.UI.Models.Dto.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
