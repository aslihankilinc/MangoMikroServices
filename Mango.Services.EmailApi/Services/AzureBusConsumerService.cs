using Azure.Messaging.ServiceBus;
using Mango.Services.EmailApi.IContract;

namespace Mango.Services.EmailApi.Services
{
    public class AzureBusConsumerService : IAzureBusConsumerService
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private ServiceBusProcessor _emailOrderPlacedProcessor;
        private ServiceBusProcessor _emailCartProcessor;
        public Task Start()
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
