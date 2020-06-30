using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoffeeMassTransit.DemoCommon
{
    public class PublicMessageConsumer : IConsumer<PublicMessage>
    {
        private readonly ILogger<PublicMessageConsumer> logger;

        public PublicMessageConsumer(ILogger<PublicMessageConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<PublicMessage> context)
        {
            logger?.LogInformation($"Received public message #{context.Message.MessageId} : {context.Message.Content}");
            await context.Publish<PublicMessageReceived>(new { context.Message.MessageId });
        }
    }
}
