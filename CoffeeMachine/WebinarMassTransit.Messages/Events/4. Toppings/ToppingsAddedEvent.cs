using MassTransit;
using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Messages
{
    public interface ToppingsAddedEvent : CorrelatedBy<Guid>
    {
        Topping[] Toppings { get; }
    }
}
