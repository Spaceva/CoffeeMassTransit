using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebinarMassTransit.DemoCommon;

namespace WebinarMassTransit.DemoServiceA
{
    public class AccessDeniedConsumer : IConsumer<AccessDenied>
    {
        private readonly ILogger<AccessDeniedConsumer> logger;

        public AccessDeniedConsumer(ILogger<AccessDeniedConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<AccessDenied> context)
        {
            logger?.LogInformation("Access has been denied !");
            return Task.CompletedTask;
        }
    }
}
