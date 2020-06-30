using Microsoft.Extensions.Logging;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.EventListener
{
    public class BaseCoffeeFinishedEventConsumer : ListenerConsumer<BaseCoffeeFinishedEvent>
    {
        public BaseCoffeeFinishedEventConsumer(ILogger<ListenerConsumer<BaseCoffeeFinishedEvent>> logger) : base(logger) { }
    }
}
