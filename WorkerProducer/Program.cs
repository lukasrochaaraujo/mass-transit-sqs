using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerProducer
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
                    services.AddMassTransit(mt => 
                    {
                        mt.UsingAmazonSqs((ctx, cfg) =>
                        {
                            cfg.Host("us-east-2", h =>
                            {
                                h.AccessKey("access-key");
                                h.SecretKey("secret-key");
                            });
                            cfg.ClearMessageDeserializers();
                            cfg.UseRawJsonSerializer();
                        });
                    });
                    services.AddMassTransitHostedService();
                    services.AddHostedService<Worker>();
                });
    }
}
