using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface AddToppingsArguments
{
    Topping[] Toppings { get; }
}