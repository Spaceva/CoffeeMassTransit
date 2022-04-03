using CoffeeMassTransit.Contracts;
using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface MakeCoffeeCommand : CorrelatedBy<Guid>
{
    Guid OrderId { get; }
    
    CoffeeType CoffeeType { get; }

    bool NoToppings { get; }
}
