using MassTransit;
using System;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using CoffeeMassTransit.SubOrchestration.Messages;

namespace CoffeeMassTransit.SubOrchestration.StateMachine;

public class CoffeeOrderStateMachine : MassTransitStateMachine<CoffeeOrderState>
{
    public CoffeeOrderStateMachine()
    {
        var consumersEndpoint = new Uri("queue:commands");
        var subOrchestrationEndpoint = new Uri("queue:sub-orchestration");

        Event(() => OrderSubmittedEvent, x => x.CorrelateById(context => context.Message.CorrelationId).SelectId(context => context.Message.CorrelationId));
        Event(() => PaymentAcceptedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentRefusedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => CoffeeServedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

        InstanceState(x => x.CurrentState);

        Initially(When(OrderSubmittedEvent)
             .Then(x =>
             {
                 x.Saga.CustomerName = x.Message.CustomerName;
                 x.Saga.CoffeeTypeRequested = x.Message.CoffeeType;
                 x.Saga.ToppingsRequested = string.Join(",", x.Message.Toppings);
                 x.Saga.Amount = CoffeePriceCalculator.Compute(x.Message.CoffeeType, x.Message.Toppings);
             })
             .SendAsync(consumersEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Amount }))
             .TransitionTo(AwaitingPayment),
             Ignore(PaymentAcceptedEvent));

        During(AwaitingPayment,
                When(PaymentAcceptedEvent)
                    .SendAsync(subOrchestrationEndpoint, context => context.Init<CreateCoffeeCommand>(new { CorrelationId = NewId.Next(), OrderId = context.Saga.CorrelationId, CoffeeType = context.Saga.CoffeeTypeRequested, Toppings = context.Saga.ToppingsRequested?.Split(',') ?? Array.Empty<string>() }))
                    .TransitionTo(Paid),
                When(PaymentRefusedEvent)
                    .SendAsync(consumersEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Amount })));

        During(Paid,
            When(CoffeeServedEvent)
                .Finalize());
    }

    public State AwaitingPayment { get; private set; } = default!;
    public State Paid { get; private set; } = default!;
    public State BaseCoffeeOK { get; private set; } = default!;
    public State CoffeeReady { get; private set; } = default!;

    public Event<OrderSubmittedEvent> OrderSubmittedEvent { get; private set; } = default!;

    public Event<PaymentAcceptedEvent> PaymentAcceptedEvent { get; private set; } = default!;

    public Event<PaymentRefusedEvent> PaymentRefusedEvent { get; private set; } = default!;

    public Event<CoffeeServedEvent> CoffeeServedEvent { get; private set; } = default!;
}
