using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages;

public interface BaseCoffeeFinishedEvent : CorrelatedBy<Guid>
{
    CoffeeType Type { get; }
}
