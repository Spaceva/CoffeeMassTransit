using MassTransit;
using System;
using CoffeeMassTransit.Messages;
using CoffeeMassTransit.SubOrchestration.Messages;
using CoffeeMassTransit.Common.AuditedStateMachine;

namespace CoffeeMassTransit.SubOrchestration.CoffeeStateMachine;

public class CoffeeStateMachine : MassTransitStateMachine<CoffeeMachineState>
{
    public CoffeeStateMachine()
    {
        var subOrchestrationCommandsConsumersEndpoints = new Uri($"queue:sub-orchestration-commands");

        Event(() => CreateCoffeeCommand, x => x.CorrelateById(context => context.Message.CorrelationId).SelectId(context => context.Message.CorrelationId));
        Event(() => CoffeeTankCheckedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => MilkTankCheckedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => CoffeeMadeEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => ToppingsAddedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

        InstanceState(x => x.CurrentState);

        Initially(When(CreateCoffeeCommand)
             .SaveInput(message => new CoffeeMachineStateInput
             {
                 CoffeeTypeRequested = message.CoffeeType,
                 ToppingsRequested = string.Join(",", message.Toppings),
                 OrderId = message.OrderId
             })
             .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<CheckCoffeeTankCommand>(new { context.Saga.CorrelationId }))
             .StartAndTransitionTo(CheckingCoffeeTank));

        During(CheckingCoffeeTank,
                When(CoffeeTankCheckedEvent)
                    .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<CheckMilkTankCommand>(new { context.Saga.CorrelationId }))
                    .SaveAndTransitionTo(CheckingMilkTank));

        During(CheckingMilkTank,
                When(MilkTankCheckedEvent)
                    .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<MakeCoffeeCommand>(new { context.Saga.CorrelationId, context.Saga.Input!.OrderId, CoffeeType = context.Saga.Input!.CoffeeTypeRequested, NoToppings = string.IsNullOrWhiteSpace(context.Saga.Input.ToppingsRequested) }))
                    .SaveAndTransitionTo(MakingCoffee));

        During(MakingCoffee,
            When(CoffeeMadeEvent)
                .IfElse(x => string.IsNullOrWhiteSpace(x.Saga.Input?.ToppingsRequested),
                    x => x.Finalize(),
                    x => x.SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<AddToppingsCommand>(new { context.Saga.CorrelationId, Toppings = context.Saga.Input!.ToppingsRequested!.Split(',') }))
                    .SaveAndTransitionTo(AddingToppings)));

        During(AddingToppings,
                When(ToppingsAddedEvent)
                    .PublishAsync(context => context.Init<CoffeeServedEvent>(new { CorrelationId = context.Saga.Input!.OrderId, CoffeeId = context.Saga.CorrelationId }))
                    .End());
    }

    public State CheckingCoffeeTank { get; private set; } = default!;
    public State CheckingMilkTank { get; private set; } = default!;
    public State MakingCoffee { get; private set; } = default!;
    public State AddingToppings { get; private set; } = default!;

    public Event<CreateCoffeeCommand> CreateCoffeeCommand { get; private set; } = default!;

    public Event<CoffeeTankCheckedEvent> CoffeeTankCheckedEvent { get; private set; } = default!;

    public Event<MilkTankCheckedEvent> MilkTankCheckedEvent { get; private set; } = default!;

    public Event<CoffeeMadeEvent> CoffeeMadeEvent { get; private set; } = default!;

    public Event<ToppingsAddedEvent> ToppingsAddedEvent { get; private set; } = default!;
}
