using Microsoft.Extensions.Logging;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.EventListener
{
    public class ToppingsAddedEventConsumer : ListenerConsumer<ToppingsAddedEvent>
    {
        public ToppingsAddedEventConsumer(ILogger<ListenerConsumer<ToppingsAddedEvent>> logger) : base(logger) { }
    }
}
