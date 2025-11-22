using Mango.Web.UI.Utility;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.UI.Models.Dto.Auth
{
    public class RegistrationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }

        public string? Role { get; set; } = Const.Customer;
    }
}
