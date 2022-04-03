using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CoffeeTankCheckedEvent : CorrelatedBy<Guid>
{
}
