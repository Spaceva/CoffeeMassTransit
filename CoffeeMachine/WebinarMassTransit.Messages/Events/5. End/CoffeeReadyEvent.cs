using MassTransit;
using System;

namespace WebinarMassTransit.Messages
{
    public interface CoffeeReadyEvent : CorrelatedBy<Guid>
    {
    }
}
