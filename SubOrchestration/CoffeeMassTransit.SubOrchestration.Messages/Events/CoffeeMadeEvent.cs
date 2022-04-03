using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CoffeeMadeEvent : CorrelatedBy<Guid>
{
}
