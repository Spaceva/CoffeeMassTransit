using MassTransit;
using System;
using CoffeeMassTransit.Contracts;

namespace RoutingSlip.Activities;

public interface OrderSubmitted : CorrelatedBy<Guid>
{
    CoffeeType CoffeeType { get; }
    Topping[] Toppings { get; }
    string CustomerName { get; }
}
