using CoffeeMassTransit.Contracts;
using MassTransit;

namespace RoutingSlip.Activities;

public interface ToppingsAdded : CorrelatedBy<Guid>
{
}
