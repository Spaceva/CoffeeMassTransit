using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceB
{
    public class InformationRequestConsumer : IConsumer<InformationRequest>
    {
        private readonly ILogger<InformationRequestConsumer> logger;

        public InformationRequestConsumer(ILogger<InformationRequestConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<InformationRequest> context)
        {
            logger?.LogInformation("Received an information request !");
            if (DateTime.Now.Second % 2 == 1)
            {
                logger?.LogInformation("Nope ! Won't happen...");
                throw new AccessViolationException();
            }
            var sendEndpoint = await context.GetSendEndpoint(new Uri("exchange:serviceA"));
            await sendEndpoint.Send<InformationResponse>(new { });
            logger?.LogInformation("Answered !");

        }
    }
}
