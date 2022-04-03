using MassTransit;
using System;
using CoffeeMassTransit.Messages;
using CoffeeMassTransit.SubOrchestration.Messages;

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
             .Then(x =>
             {
                 x.Saga.CoffeeTypeRequested = x.Message.CoffeeType;
                 x.Saga.ToppingsRequested = string.Join(",", x.Message.Toppings);
                 x.Saga.OrderId = x.Message.OrderId;
             })
             .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<CheckCoffeeTankCommand>(new { context.Saga.CorrelationId }))
             .TransitionTo(CheckingCoffeeTank));

        During(CheckingCoffeeTank,
                When(CoffeeTankCheckedEvent)
                    .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<CheckMilkTankCommand>(new { context.Saga.CorrelationId }))
                    .TransitionTo(CheckingMilkTank));

        During(CheckingMilkTank,
                When(MilkTankCheckedEvent)
                    .SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<MakeCoffeeCommand>(new { context.Saga.CorrelationId, context.Saga.OrderId, CoffeeType = context.Saga.CoffeeTypeRequested, NoToppings = string.IsNullOrWhiteSpace(context.Saga.ToppingsRequested) }))
                    .TransitionTo(MakingCoffee));

        During(MakingCoffee,
            When(CoffeeMadeEvent)
                .IfElse(x => string.IsNullOrWhiteSpace(x.Saga.ToppingsRequested),
                    x => x.Finalize(),
                    x => x.SendAsync(subOrchestrationCommandsConsumersEndpoints, context => context.Init<AddToppingsCommand>(new { context.Saga.CorrelationId, Toppings = context.Saga.ToppingsRequested!.Split(',') }))
                    .TransitionTo(AddingToppings)));

        During(AddingToppings,
                When(ToppingsAddedEvent)
                    .PublishAsync(context => context.Init<CoffeeServedEvent>(new { CorrelationId = context.Saga.OrderId, CoffeeId = context.Saga.CorrelationId }))
                    .Finalize());
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
