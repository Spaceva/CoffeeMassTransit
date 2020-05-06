using MassTransit;
using System;

namespace WebinarMassTransit.Messages
{
    public interface PaymentAcceptedEvent : CorrelatedBy<Guid>
    {
    }
}
