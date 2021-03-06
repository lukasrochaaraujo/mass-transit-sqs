using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WorkerConsumer
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(10000, stoppingToken);
        }
    }
}
