namespace Mango.Services.EmailApi.IContract
{
    public interface IAzureBusConsumerService
    {
        Task Start();
        Task Stop();
    }
}
