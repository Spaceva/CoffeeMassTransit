using MassTransit;
using System;
using System.Linq;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.StateMachine;

public class CoffeeStateMachine : MassTransitStateMachine<CoffeeState>
{
    public CoffeeStateMachine()
    {
        var requestPaymentEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(RequestPaymentCommand))}");
        var createBaseCoffeeEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(CreateBaseCoffeeCommand))}");
        var addToppingsEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(AddToppingsCommand))}");


        Event(() => OrderSubmittedEvent, x => x.CorrelateById(context => context.Message.CorrelationId).SelectId(context => context.Message.CorrelationId));
        Event(() => PaymentAcceptedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentRefusedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => BaseCoffeeFinishedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => ToppingsAddedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

        InstanceState(x => x.CurrentState);

        Initially(When(OrderSubmittedEvent)
             .Then(x =>
             {
                 x.Saga.CustomerName = x.Message.CustomerName;
                 x.Saga.CoffeeTypeRequested = x.Message.CoffeeType;
                 x.Saga.ToppingsRequested = string.Join(",", x.Message.Toppings);
                 x.Saga.Amount = CoffeePriceCalculator.Compute(x.Message.CoffeeType, x.Message.Toppings);
             })
             .SendAsync(requestPaymentEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Amount }))
             .TransitionTo(AwaitingPayment),
             Ignore(PaymentAcceptedEvent));

        During(AwaitingPayment,
                When(PaymentAcceptedEvent)
                    .SendAsync(createBaseCoffeeEndpoint, context => context.Init<CreateBaseCoffeeCommand>(new { context.Saga.CorrelationId, CoffeeType = context.Saga.CoffeeTypeRequested, NoTopping = string.IsNullOrWhiteSpace(context.Saga.ToppingsRequested) }))
                    .TransitionTo(Paid),
                When(PaymentRefusedEvent)
                    .SendAsync(requestPaymentEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Saga.CorrelationId, context.Saga.Amount })));

        During(Paid, When(BaseCoffeeFinishedEvent)
            .IfElse(context => !string.IsNullOrWhiteSpace(context.Saga.ToppingsRequested), x => x
                    .SendAsync(addToppingsEndpoint, context => context.Init<AddToppingsCommand>(new { context.Saga.CorrelationId, Toppings = context.Saga.ToppingsRequested!.Split(",").Select(t => Enum.Parse<Topping>(t)) }))
                    .TransitionTo(BaseCoffeeOK), 
                    x => x.Finalize()));

        During(BaseCoffeeOK, When(ToppingsAddedEvent)
                            .Finalize());
    }

    public State AwaitingPayment { get; private set; } = default!;
    public State Paid { get; private set; } = default!;
    public State BaseCoffeeOK { get; private set; } = default!;
    public State CoffeeReady { get; private set; } = default!;

    public Event<OrderSubmittedEvent> OrderSubmittedEvent { get; private set; } = default!;

    public Event<PaymentAcceptedEvent> PaymentAcceptedEvent { get; private set; } = default!;

    public Event<PaymentRefusedEvent> PaymentRefusedEvent { get; private set; } = default!;

    public Event<BaseCoffeeFinishedEvent> BaseCoffeeFinishedEvent { get; private set; } = default!;

    public Event<ToppingsAddedEvent> ToppingsAddedEvent { get; private set; } = default!;
}
