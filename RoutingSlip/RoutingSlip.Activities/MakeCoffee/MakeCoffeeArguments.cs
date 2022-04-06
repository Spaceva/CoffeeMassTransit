using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface MakeCoffeeArguments
{
    CoffeeType CoffeeType { get; }
    bool NoToppings { get; }
}