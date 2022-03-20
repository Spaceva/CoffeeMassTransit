using Microsoft.Extensions.Logging;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.EventListener;

public class PaymentRefusedEventConsumer : ListenerConsumer<PaymentRefusedEvent>
{
    public PaymentRefusedEventConsumer(ILogger<ListenerConsumer<PaymentRefusedEvent>> logger) : base(logger) { }
}
