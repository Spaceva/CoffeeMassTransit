using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class ToppingsAddedEventConsumer : ListenerConsumer<ToppingsAddedEvent>
    {
        public ToppingsAddedEventConsumer(ILogger<ListenerConsumer<ToppingsAddedEvent>> logger) : base(logger) { }
    }
}
