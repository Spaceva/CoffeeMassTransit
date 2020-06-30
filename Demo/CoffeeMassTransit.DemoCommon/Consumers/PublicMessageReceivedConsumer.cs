using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoffeeMassTransit.DemoCommon
{
    public class PublicMessageReceivedConsumer : IConsumer<PublicMessageReceived>
    {
        private readonly ILogger<PublicMessageReceivedConsumer> logger;

        public PublicMessageReceivedConsumer(ILogger<PublicMessageReceivedConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<PublicMessageReceived> context)
        {
            logger?.LogInformation($"My message has been received ! Cool !");
            return Task.CompletedTask;
        }
    }
}