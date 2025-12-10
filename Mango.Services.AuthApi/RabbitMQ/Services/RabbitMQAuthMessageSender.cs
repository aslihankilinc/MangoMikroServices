using Mango.Services.AuthApi.RabbitMQ.IContract;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.AuthApi.RabbitMQ.Services
{
    public class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection connection;
        public RabbitMQAuthMessageSender()
        {
            _hostName = "localhost";
            _password= "guest";
            _userName = "guest";
        }
        public async void SendMessage(object mess, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            //RabbitMq ile baglanti kur
            connection = await factory.CreateConnectionAsync();
            using var channel =await  connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                     durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var json=JsonConvert.SerializeObject(mess);
            var body =Encoding.UTF8.GetBytes(json);
            //mesaji yayinlama
            await channel.BasicPublishAsync(exchange:"",routingKey:queueName,body:body);

        }
    }
}
