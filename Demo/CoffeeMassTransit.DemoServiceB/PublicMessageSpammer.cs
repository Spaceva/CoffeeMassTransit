using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceB
{
    public class PublicMessageSpammer : BackgroundService
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<PublicMessageSpammer> logger;

        public PublicMessageSpammer(IPublishEndpoint publishEndpoint, ILogger<PublicMessageSpammer> logger)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger?.LogInformation("Starting...");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(17), stoppingToken);
                await publishEndpoint.Publish<PublicMessage>(new { MessageId = Guid.NewGuid(), Content = "Hello from Service B" }, stoppingToken);
                logger?.LogInformation("PublicMessage sent");
            }
        }
    }
}
