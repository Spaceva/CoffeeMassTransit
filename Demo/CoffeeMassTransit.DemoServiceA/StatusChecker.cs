using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceA
{
    public class StatusChecker : BackgroundService
    {
        private readonly IRequestClient<StatusCheck> requestClient;
        private readonly ILogger<StatusChecker> logger;

        public StatusChecker(IRequestClient<StatusCheck> requestClient, ILogger<StatusChecker> logger)
        {
            this.requestClient = requestClient;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger?.LogInformation("Starting...");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken);
                logger?.LogInformation("Let's ask how he feels..");
                var (responseOKTask, responseKOTask) = await requestClient.GetResponse<StatusOKResponse, StatusNOKResponse>(new { }, stoppingToken);
                if (!responseOKTask.IsCompletedSuccessfully)
                {
                    var responseKO = await responseKOTask;
                    logger?.LogInformation($"Oh no ! He answered '{responseKO.Message.Reason}' !");
                    continue;
                }

                logger?.LogInformation("It's OK!");
            }
        }
    }
}
