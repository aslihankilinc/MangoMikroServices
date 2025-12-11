using Mango.Services.EmailApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mango.Services.EmailApi.Services
{
    public class RabbitMQAuthConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IChannel _channel;
        public RabbitMQAuthConsumerService(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection =  factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _channel.QueueDeclareAsync(_configuration.GetValue<string>("RabbitMQEmailSettings:AuthQueueName")
                , true, false, false, null);

        }
        


        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {              

                consumer.ReceivedAsync += async (ch, ea) =>
                {
                    try
                    {
                        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                        string email = content;

                        Console.WriteLine($"Rabbit Mail :{email}");
                        await HandleMessage(email);

                        await _channel.BasicAckAsync(ea.DeliveryTag, false);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Rabbit Mail Hata:{ex.ToString()}"
                     );
                    }
                   
                };

                _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

          await  _channel.BasicConsumeAsync(_configuration.GetValue<string>("RabbitMQEmailSettings:AuthQueueName"), false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string email)
        {
            _emailService.RegisterUserEmailAndLog(email).GetAwaiter().GetResult();
        }
    }
}
