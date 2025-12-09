using Azure.Messaging.ServiceBus;
using Mango.Services.EmailApi.IContract;
using Mango.Services.EmailApi.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailApi.Services
{
    public class AzureBusConsumerService : IAzureBusConsumerService
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailQueue;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailProcessor;

        public AzureBusConsumerService(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailProcessor = client.CreateProcessor(emailQueue);
        }

        public async Task Start()
        {
            _emailProcessor.ProcessMessageAsync += OnEmailRequestReceived;
            _emailProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailProcessor.StartProcessingAsync();

        }

        public async Task Stop()
        {
            await _emailProcessor.StopProcessingAsync();
            await _emailProcessor.DisposeAsync();
        }


        private async Task OnEmailRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //TODO - try to log email
                await _emailService.EmailLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}

