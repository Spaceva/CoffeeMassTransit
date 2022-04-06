using MassTransit;

namespace RoutingSlip.Activities;

public interface CoffeeServed : CorrelatedBy<Guid>
{
}
