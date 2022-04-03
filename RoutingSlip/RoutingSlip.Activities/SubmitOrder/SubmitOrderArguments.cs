using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface SubmitOrderArguments
{
    string CustomerName { get; }
    string? ToppingsRequested { get; }
    CoffeeType CoffeeTypeRequested { get; }
    float Amount { get; }
}