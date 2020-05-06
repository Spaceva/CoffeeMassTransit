using MassTransit;
using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Messages
{
    public interface BaseCoffeeFinishedEvent : CorrelatedBy<Guid>
    {
        CoffeeType Type { get; }
    }
}
