using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CreateCoffeeCommand : CorrelatedBy<Guid>
{
    Guid OrderId { get; }

    CoffeeType CoffeeType { get; }

    Topping[] Toppings { get; }
}
