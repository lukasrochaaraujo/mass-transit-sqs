using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();
                        x.UsingAmazonSqs((context, cfg) =>
                        {
                            cfg.Host("us-east-2", h =>
                            {
                                h.AccessKey("access-key");
                                h.SecretKey("secret-key");
                            });
                            cfg.ReceiveEndpoint("message", e =>
                            {
                                e.ConfigureConsumeTopology = false;
                                e.ClearMessageDeserializers();
                                e.UseRawJsonSerializer();
                                e.RethrowFaultedMessages();
                                e.ConfigureConsumer<MessageConsumer>(context, cc => 
                                {
                                    cc.UseConcurrentMessageLimit(2);
                                });
                            });
                        });
                    });
                    services.AddMassTransitHostedService();
                    services.AddHostedService<Worker>();
                });
    }
}
