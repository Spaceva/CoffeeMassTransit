using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages
{
    public interface OrderSubmittedEvent : CorrelatedBy<Guid>
    {
        CoffeeType CoffeeType { get; }
        Topping[] Toppings { get; }
        string CustomerName { get; }
    }
}
