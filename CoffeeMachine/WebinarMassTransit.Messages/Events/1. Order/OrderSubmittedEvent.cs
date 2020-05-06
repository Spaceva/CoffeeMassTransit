using MassTransit;
using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Messages
{
    public interface OrderSubmittedEvent : CorrelatedBy<Guid>
    {
        CoffeeType CoffeeType { get; }
        Topping[] Toppings { get; }
        string CustomerName { get; }
    }
}
