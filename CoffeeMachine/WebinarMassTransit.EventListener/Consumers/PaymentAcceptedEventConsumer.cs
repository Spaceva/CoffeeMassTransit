using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class PaymentAcceptedEventConsumer : ListenerConsumer<PaymentAcceptedEvent>
    {
        public PaymentAcceptedEventConsumer(ILogger<ListenerConsumer<PaymentAcceptedEvent>> logger) : base(logger) { }
    }
}
