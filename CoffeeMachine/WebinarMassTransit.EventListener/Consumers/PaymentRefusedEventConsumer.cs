using Microsoft.Extensions.Logging;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.EventListener
{
    public class PaymentRefusedEventConsumer : ListenerConsumer<PaymentRefusedEvent>
    {
        public PaymentRefusedEventConsumer(ILogger<ListenerConsumer<PaymentRefusedEvent>> logger) : base(logger) { }
    }
}
