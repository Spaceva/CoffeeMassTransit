using Dapper.Contrib.Extensions;
using System;
using CoffeeMassTransit.Contracts;
using MassTransit;

namespace CoffeeMassTransit.StateMachine;

public class CoffeeState : SagaStateMachineInstance
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string? ToppingsRequested { get; set; }
    public CoffeeType CoffeeTypeRequested { get; set; }
    public float Amount { get; set; }
}
