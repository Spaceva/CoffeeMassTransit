using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Messages
{
    public interface CreateBaseCoffeeCommand : CorrelatedBy<Guid>
    {
        CoffeeType CoffeeType { get; }

        bool NoTopping { get; }
    }
}
