using Microsoft.Extensions.Logging;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.EventListener
{
    public class PaymentAcceptedEventConsumer : ListenerConsumer<PaymentAcceptedEvent>
    {
        public PaymentAcceptedEventConsumer(ILogger<ListenerConsumer<PaymentAcceptedEvent>> logger) : base(logger) { }
    }
}
