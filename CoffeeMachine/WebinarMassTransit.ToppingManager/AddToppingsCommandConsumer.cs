using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebinarMassTransit.Core;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.ToppingManager
{
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
            this.logger?.LogInformation($"Consuming AddToppingsCommand - {context.Message.CorrelationId}");
            await Task.Delay(TimeSpan.FromSeconds(8));
            this.coffeeRepository.AddToppings(context.CorrelationId.Value, context.Message.Toppings);
            await context.Publish<ToppingsAddedEvent>(new { context.CorrelationId, context.Message.Toppings });
        }
    }
}
