using MassTransit;
using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Messages
{
    public interface CreateBaseCoffeeCommand : CorrelatedBy<Guid>
    {
        CoffeeType CoffeeType { get; }

        bool NoTopping { get; }
    }
}
