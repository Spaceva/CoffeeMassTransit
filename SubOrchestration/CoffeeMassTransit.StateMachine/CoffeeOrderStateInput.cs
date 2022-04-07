using CoffeeMassTransit.Common.AuditedStateMachine;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.SubOrchestration.StateMachine;

public class CoffeeOrderStateInput
{
    public string CustomerName { get; init; } = default!;
    public string? ToppingsRequested { get; init; }
    public CoffeeType CoffeeTypeRequested { get; init; }
    public float Amount { get; init; }
}