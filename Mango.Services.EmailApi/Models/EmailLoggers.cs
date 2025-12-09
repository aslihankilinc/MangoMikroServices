using System.ComponentModel.DataAnnotations;

namespace Mango.Services.EmailApi.Models
{
    public class EmailLoggers
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string EmailSent { get; set; }
    }
}
