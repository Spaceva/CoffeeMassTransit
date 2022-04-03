using System;
using CoffeeMassTransit.Contracts;
using Dapper.Contrib.Extensions;
using MassTransit;

namespace CoffeeMassTransit.SubOrchestration.CoffeeStateMachine;

public class CoffeeMachineState : SagaStateMachineInstance
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
    public string CurrentState { get; set; } = default!;
    public string? ToppingsRequested { get; set; }
    public CoffeeType CoffeeTypeRequested { get; set; }
}
