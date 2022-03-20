using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages;

public interface AddToppingsCommand : CorrelatedBy<Guid>
{
    Topping[] Toppings { get; }
}
