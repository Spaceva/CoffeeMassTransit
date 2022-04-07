using MassTransit;

namespace RoutingSlip.Activities;

public interface ToppingsAdded : CorrelatedBy<Guid>
{
}
