using Microsoft.Extensions.Logging;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.EventListener;

public class OrderSubmittedEventConsumer : ListenerConsumer<OrderSubmittedEvent>
{
    public OrderSubmittedEventConsumer(ILogger<ListenerConsumer<OrderSubmittedEvent>> logger) : base(logger) { }
}
