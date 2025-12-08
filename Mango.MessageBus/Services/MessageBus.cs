using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus.Services
{
    public class MessageBus
    {
        private string connectionString = "Endpoint=/subscriptions/48f26535-dab5-47fd-a1a1-a7ddcec8b6e3/resourceGroups/DotNetMaster/providers/Microsoft.Resources/deployments/webproject/operations/91C18CFDB2D387B4";
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
