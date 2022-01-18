using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceA
{
    public class InformationResponseConsumer : IConsumer<InformationResponse>
    {
        private readonly ILogger<InformationResponseConsumer> logger;

        public InformationResponseConsumer(ILogger<InformationResponseConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<InformationResponse> context)
        {
            logger?.LogInformation("Received the information !");
            return Task.CompletedTask;
        }
    }
}
