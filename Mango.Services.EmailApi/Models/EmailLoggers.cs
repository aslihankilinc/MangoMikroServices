namespace Mango.Services.EmailApi.Models
{
    public class EmailLoggers
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime? EmailSent { get; set; }
    }
}
