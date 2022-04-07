using CoffeeMassTransit.SubOrchestration.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.SubOrchestration.Consumers;
public class CheckCoffeeTankCommandConsumer : IConsumer<CheckCoffeeTankCommand>
{
    private readonly ILogger<CheckCoffeeTankCommandConsumer> logger;

    public CheckCoffeeTankCommandConsumer(ILogger<CheckCoffeeTankCommandConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<CheckCoffeeTankCommand> context)
    {
        logger.LogInformation("Checking Coffee tank...");
        await Task.Delay(TimeSpan.FromSeconds(3));
        logger.LogInformation("Coffee tank OK.");
        await context.Publish<CoffeeTankCheckedEvent>(new { context.Message.CorrelationId });
    }
}
