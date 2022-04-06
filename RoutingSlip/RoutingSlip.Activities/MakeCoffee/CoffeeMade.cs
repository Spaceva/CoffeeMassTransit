using MassTransit;

namespace RoutingSlip.Activities;

public interface CoffeeMade : CorrelatedBy<Guid>
{
}
