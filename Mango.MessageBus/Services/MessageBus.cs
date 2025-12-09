using Azure.Messaging.ServiceBus;
using Mango.MessageBus.IContract;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus.Services
{
    public class MessageBus:IMessageBus
    {
        private string connectionString = "Endpoint=sb://webproject.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FaIdu2D/BMFr4yZBgG3vFijmn5DGrlRRB+ASbCKAeAg=";
       public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);
            var body = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBus = new ServiceBusMessage
                                       (Encoding
                .UTF8.GetBytes(body))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };


            await sender.SendMessageAsync(serviceBus);
            await client.DisposeAsync();
        }

    }
}
