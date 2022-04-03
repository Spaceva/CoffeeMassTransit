using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages;

public interface CreateBaseCoffeeCommand : CorrelatedBy<Guid>
{
    Guid OrderId { get; }
    
    CoffeeType CoffeeType { get; }

    bool NoTopping { get; }
}
