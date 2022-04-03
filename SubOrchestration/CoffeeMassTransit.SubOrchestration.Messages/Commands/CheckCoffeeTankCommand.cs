using MassTransit;
using System;

namespace CoffeeMassTransit.SubOrchestration.Messages;

public interface CheckCoffeeTankCommand : CorrelatedBy<Guid>
{
}
