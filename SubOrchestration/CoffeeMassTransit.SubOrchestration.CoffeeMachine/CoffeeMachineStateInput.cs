using CoffeeMassTransit.Contracts;
using System;

namespace CoffeeMassTransit.SubOrchestration.CoffeeStateMachine;

public class CoffeeMachineStateInput
{
    public Guid OrderId { get; set; }
    public string? ToppingsRequested { get; init; }
    public CoffeeType CoffeeTypeRequested { get; init; }
}
