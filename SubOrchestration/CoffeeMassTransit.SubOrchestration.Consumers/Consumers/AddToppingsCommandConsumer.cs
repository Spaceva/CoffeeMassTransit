using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.SubOrchestration.Consumers;
public class AddToppingsCommandConsumer : IConsumer<AddToppingsCommand>
{
    private readonly ICoffeeRepository coffeeRepository;
    private readonly ILogger<AddToppingsCommandConsumer> logger;

    public AddToppingsCommandConsumer(ICoffeeRepository coffeeRepository, ILogger<AddToppingsCommandConsumer> logger)
    {
        this.coffeeRepository = coffeeRepository;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<AddToppingsCommand> context)
    {
        this.logger.LogInformation("Addings Toppings: {Toppings}", string.Join(',', context.Message.Toppings));
        await Task.Delay(TimeSpan.FromSeconds(3));
        coffeeRepository.AddToppings(context.CorrelationId!.Value, context.Message.Toppings);
        this.logger.LogInformation("Done.");
        await context.Publish<ToppingsAddedEvent>(new { context.Message.CorrelationId, context.Message.Toppings });
    }
}
