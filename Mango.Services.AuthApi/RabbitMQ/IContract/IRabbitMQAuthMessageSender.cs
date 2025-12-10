namespace Mango.Services.AuthApi.RabbitMQ.IContract
{
    public interface IRabbitMQAuthMessageSender
    {
        void SendMessage(Object mess, string queueName);
    }
}
