using MassTransit;
using System;

namespace CoffeeMassTransit.Messages
{
    public interface PaymentRefusedEvent : CorrelatedBy<Guid>
    {
        public string Error { get; }
    }
}
