using MassTransit;
using System;

namespace WebinarMassTransit.Messages
{
    public interface PaymentRefusedEvent : CorrelatedBy<Guid>
    {
        public string Error { get; }
    }
}
