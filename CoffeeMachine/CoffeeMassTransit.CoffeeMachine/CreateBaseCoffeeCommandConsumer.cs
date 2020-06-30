using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.CoffeeMachine
{
    public class CreateBaseCoffeeCommandConsumer : IConsumer<CreateBaseCoffeeCommand>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly ILogger<CreateBaseCoffeeCommandConsumer> logger;

        public CreateBaseCoffeeCommandConsumer(ICoffeeRepository coffeeRepository, ILogger<CreateBaseCoffeeCommandConsumer> logger)
        {
            this.coffeeRepository = coffeeRepository;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateBaseCoffeeCommand> context)
        {
            this.logger?.LogInformation($"Consuming CreateBaseCoffeeCommand - {context.Message.CorrelationId}");
            if (!(logger is null))
                await Task.Delay(TimeSpan.FromSeconds(12));
            if (DateTime.Now.Second % 7 == 0)
                throw new EmptyTankException("Empty Tank. Please refill");
            coffeeRepository.Create(context.CorrelationId.Value, context.Message.CoffeeType, context.Message.NoTopping);
            await context.Publish<BaseCoffeeFinishedEvent>(new { context.CorrelationId });
        }
    }
}
