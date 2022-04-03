using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface MilkTankCheckedEvent : CorrelatedBy<Guid>
{
}
