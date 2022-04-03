using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface PaymentAccepted : CorrelatedBy<Guid>
{
}
