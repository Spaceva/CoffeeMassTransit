using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class CoffeeReadyEventConsumer : ListenerConsumer<CoffeeReadyEvent>
    {
        public CoffeeReadyEventConsumer(ILogger<ListenerConsumer<CoffeeReadyEvent>> logger) : base(logger) { }
    }
}
