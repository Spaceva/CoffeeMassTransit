using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages;

public interface ToppingsAddedEvent : CorrelatedBy<Guid>
{
    Topping[] Toppings { get; }
}
