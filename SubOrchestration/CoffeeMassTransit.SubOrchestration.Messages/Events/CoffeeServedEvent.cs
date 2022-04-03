using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CoffeeServedEvent : CorrelatedBy<Guid>
{
    Guid CoffeeId { get; }
}
