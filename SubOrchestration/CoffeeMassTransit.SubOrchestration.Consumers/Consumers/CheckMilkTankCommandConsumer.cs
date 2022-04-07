using CoffeeMassTransit.SubOrchestration.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.SubOrchestration.Consumers;
public class CheckMilkTankCommandConsumer : IConsumer<CheckMilkTankCommand>
{
    private readonly ILogger<CheckMilkTankCommandConsumer> logger;

    public CheckMilkTankCommandConsumer(ILogger<CheckMilkTankCommandConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<CheckMilkTankCommand> context)
    {
        logger.LogInformation("Checking Milk tank...");
        await Task.Delay(TimeSpan.FromSeconds(3));
        logger.LogInformation("Milk tank OK.");
        await context.Publish<MilkTankCheckedEvent>(new { context.Message.CorrelationId });
    }
}
