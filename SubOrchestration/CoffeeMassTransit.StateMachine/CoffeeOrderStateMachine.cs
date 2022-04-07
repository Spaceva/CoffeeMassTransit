using MassTransit;
using System;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using CoffeeMassTransit.SubOrchestration.Messages;
using CoffeeMassTransit.Common.AuditedStateMachine;

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
             .SaveInput((message) => new CoffeeOrderStateInput
             {
                 CustomerName = message.CustomerName,
                 CoffeeTypeRequested = message.CoffeeType,
                 ToppingsRequested = string.Join(",", message.Toppings),
                 Amount = CoffeePriceCalculator.Compute(message.CoffeeType, message.Toppings),
             })
             .SendAsync(consumersEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Input!.Amount }))
             .StartAndTransitionTo(AwaitingPayment));

        During(AwaitingPayment,
                When(PaymentAcceptedEvent)
                    .SendAsync(subOrchestrationEndpoint, context => context.Init<CreateCoffeeCommand>(new { CorrelationId = NewId.Next(), OrderId = context.Saga.CorrelationId, CoffeeType = context.Saga.Input!.CoffeeTypeRequested, Toppings = context.Saga.Input.ToppingsRequested?.Split(',') ?? Array.Empty<string>() }))
                    .SaveAndTransitionTo(Paid),
                When(PaymentRefusedEvent)
                    .SendAsync(consumersEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Input!.Amount }))
                    .Save());

        During(Paid,
            When(CoffeeServedEvent)
                .End());
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
