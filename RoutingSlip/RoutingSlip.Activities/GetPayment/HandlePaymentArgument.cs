using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface HandlePaymentArgument
{
    float Amount { get; }
}