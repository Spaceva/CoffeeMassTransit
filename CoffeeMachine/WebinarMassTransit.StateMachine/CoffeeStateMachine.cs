using Automatonymous;
using MassTransit;
using MassTransit.Definition;
using System;
using System.Linq;
using WebinarMassTransit.Contracts;
using WebinarMassTransit.Core;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.StateMachine
{
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
                     x.Instance.CustomerName = x.Data.CustomerName;
                     x.Instance.CoffeeTypeRequested = x.Data.CoffeeType;
                     x.Instance.ToppingsRequested = string.Join(",", x.Data.Toppings);
                     x.Instance.Amount = CoffeePriceCalculator.Compute(x.Data.CoffeeType, x.Data.Toppings);
                 })
                 .SendAsync(requestPaymentEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Instance.CorrelationId, context.Instance.Amount }))
                 .TransitionTo(AwaitingPayment),
                 Ignore(PaymentAcceptedEvent));

            During(AwaitingPayment,
                    When(PaymentAcceptedEvent)
                        .SendAsync(createBaseCoffeeEndpoint, context => context.Init<CreateBaseCoffeeCommand>(new { context.Instance.CorrelationId, CoffeeType = context.Instance.CoffeeTypeRequested, NoTopping = string.IsNullOrWhiteSpace(context.Instance.ToppingsRequested) }))
                        .TransitionTo(Paid),
                    When(PaymentRefusedEvent)
                        .SendAsync(requestPaymentEndpoint, context => context.Init<RequestPaymentCommand>(new { context.Instance.CorrelationId, context.Instance.Amount })));

            During(Paid, When(BaseCoffeeFinishedEvent)
                .IfElse(context => !string.IsNullOrWhiteSpace(context.Instance.ToppingsRequested), x => x
                        .SendAsync(addToppingsEndpoint, context => context.Init<AddToppingsCommand>(new { context.Instance.CorrelationId, Toppings = context.Instance.ToppingsRequested.Split(",").Select(t => Enum.Parse<Topping>(t)) }))
                        .TransitionTo(BaseCoffeeOK), x => x
                        .PublishAsync(context => context.Init<CoffeeReadyEvent>(new { context.Instance.CorrelationId }))
                        .Finalize()));

            During(BaseCoffeeOK, When(ToppingsAddedEvent)
                                .PublishAsync(context => context.Init<CoffeeReadyEvent>(new { context.Instance.CorrelationId }))
                                .Finalize());
        }

        public State AwaitingPayment { get; private set; }
        public State Paid { get; private set; }
        public State BaseCoffeeOK { get; private set; }
        public State CoffeeReady { get; private set; }

        public Event<OrderSubmittedEvent> OrderSubmittedEvent { get; private set; }

        public Event<PaymentAcceptedEvent> PaymentAcceptedEvent { get; private set; }

        public Event<PaymentRefusedEvent> PaymentRefusedEvent { get; private set; }

        public Event<BaseCoffeeFinishedEvent> BaseCoffeeFinishedEvent { get; private set; }

        public Event<ToppingsAddedEvent> ToppingsAddedEvent { get; private set; }
    }
}
