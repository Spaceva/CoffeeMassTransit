using MassTransit;
using System;

namespace CoffeeMassTransit.Messages
{
    public interface RequestPaymentCommand : CorrelatedBy<Guid>
    {
        float Amount { get; set; }
    }
}
