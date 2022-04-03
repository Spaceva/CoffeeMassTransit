using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CheckMilkTankCommand : CorrelatedBy<Guid>
{
}
