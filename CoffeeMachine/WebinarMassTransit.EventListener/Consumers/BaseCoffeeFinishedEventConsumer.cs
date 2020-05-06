using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class BaseCoffeeFinishedEventConsumer : ListenerConsumer<BaseCoffeeFinishedEvent>
    {
        public BaseCoffeeFinishedEventConsumer(ILogger<ListenerConsumer<BaseCoffeeFinishedEvent>> logger) : base(logger) { }
    }
}
