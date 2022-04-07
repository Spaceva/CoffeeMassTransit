using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.SubOrchestration.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.SubOrchestration.Consumers;
public class MakeCoffeeCommandConsumer : IConsumer<MakeCoffeeCommand>
{
    private readonly ICoffeeRepository coffeeRepository;
    private readonly ILogger<MakeCoffeeCommandConsumer> logger;

    public MakeCoffeeCommandConsumer(ICoffeeRepository coffeeRepository, ILogger<MakeCoffeeCommandConsumer> logger)
    {
        this.coffeeRepository = coffeeRepository;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<MakeCoffeeCommand> context)
    {
        logger.LogInformation("Making coffee...");
        await Task.Delay(TimeSpan.FromSeconds(3));
        coffeeRepository.Create(context.CorrelationId!.Value, context.Message.OrderId, context.Message.CoffeeType, context.Message.NoToppings);
        logger.LogInformation("Done.");
        await context.Publish<CoffeeMadeEvent>(new { context.Message.CorrelationId });
    }
}
