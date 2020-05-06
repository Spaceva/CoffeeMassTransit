using MassTransit;
using System;

namespace WebinarMassTransit.Messages
{
    public interface RequestPaymentCommand : CorrelatedBy<Guid>
    {
        float Amount { get; set; }
    }
}
