using Mango.Services.EmailApi.IContract;

namespace Mango.Services.EmailApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureBusConsumerService serviceBusConsumer {  get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            serviceBusConsumer = app.ApplicationServices.GetService<IAzureBusConsumerService>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApplicationLifetime.ApplicationStarted.Register(OnStart);
            hostApplicationLifetime.ApplicationStopping.Register(OnStop);
            return app;
        }
        private static void OnStop()
        {
            serviceBusConsumer.Stop();
        }

        private static void OnStart()
        {
            serviceBusConsumer.Start();
        }

    }
}
