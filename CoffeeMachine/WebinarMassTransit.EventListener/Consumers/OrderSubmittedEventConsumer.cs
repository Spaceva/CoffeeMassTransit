using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class OrderSubmittedEventConsumer : ListenerConsumer<OrderSubmittedEvent>
    {
        public OrderSubmittedEventConsumer(ILogger<ListenerConsumer<OrderSubmittedEvent>> logger) : base(logger) { }
    }
}
