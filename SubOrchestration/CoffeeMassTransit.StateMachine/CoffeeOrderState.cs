using Dapper.Contrib.Extensions;
using System;
using MassTransit;
using CoffeeMassTransit.Common.AuditedStateMachine;

namespace CoffeeMassTransit.SubOrchestration.StateMachine;

public class CoffeeOrderState : AuditedState<CoffeeOrderStateInput>, SagaStateMachineInstance, ISagaVersion
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    public int Version { get; set; }
}
