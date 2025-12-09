using Mango.Services.EmailApi.Data;
using Mango.Services.EmailApi.IContract;
using Mango.Services.EmailApi.Models;
using Mango.Services.EmailApi.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailApi.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task EmailLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Sepetiniz ");
            message.AppendLine("<br/>Toplam " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await EmailLoggerSend(message.ToString(), cartDto.CartHeader.Email);
        }
     
        private async Task<bool> EmailLoggerSend(string message, string email)
        {
            try
            {
                EmailLoggers emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now.ToString(),
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
