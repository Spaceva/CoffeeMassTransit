using System;
using CoffeeMassTransit.Common.AuditedStateMachine;
using Dapper.Contrib.Extensions;
using MassTransit;

namespace CoffeeMassTransit.SubOrchestration.CoffeeStateMachine;

public class CoffeeMachineState : AuditedState<CoffeeMachineStateInput>, SagaStateMachineInstance, ISagaVersion
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    public int Version { get; set; }
}
