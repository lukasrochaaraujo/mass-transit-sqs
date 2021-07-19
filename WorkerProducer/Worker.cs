using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;

namespace WorkerProducer
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly ILogger<Worker> _logger;

        public Worker(IBus bus, ILogger<Worker> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sender = await _bus.GetSendEndpoint(new Uri("queue:message"));

            while (!stoppingToken.IsCancellationRequested)
            {
                await sender.Send(new Message() { Text = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
                _logger.LogInformation($"Message sended at {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
