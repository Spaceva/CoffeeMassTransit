using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebinarMassTransit.Contracts;
using WebinarMassTransit.Core;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.WebinarMassTransit.ConsumerSaga
{
    public class CoffeeMachineSaga : ISaga,
        InitiatedBy<OrderSubmittedEvent>,
        Orchestrates<PaymentAcceptedEvent>,
        Orchestrates<PaymentRefusedEvent>,
        Orchestrates<BaseCoffeeFinishedEvent>,
        Orchestrates<ToppingsAddedEvent>

    {
        public Guid CorrelationId { get; set; }
        public string CustomerName { get; set; }
        public string ToppingsRequested { get; set; }
        public CoffeeType CoffeeTypeRequested { get; set; }
        public float Amount { get; set; }

        public string State { get; private set; } = "Not Started";

        public readonly Uri requestPaymentEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(RequestPaymentCommand))}");
        public readonly Uri createBaseCoffeeEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(CreateBaseCoffeeCommand))}");
        public readonly Uri addToppingsEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(AddToppingsCommand))}");

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            this.CustomerName = context.Message.CustomerName;
            this.CoffeeTypeRequested = context.Message.CoffeeType;
            this.ToppingsRequested = string.Join(",", context.Message.Toppings);
            this.Amount = CoffeePriceCalculator.Compute(context.Message.CoffeeType, context.Message.Toppings);
            var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
            await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount });
            this.State = "AwaitingPayment";
        }

        public async Task Consume(ConsumeContext<PaymentAcceptedEvent> context)
        {
            var sendEndpoint = await context.GetSendEndpoint(createBaseCoffeeEndpoint);
            await sendEndpoint.Send<CreateBaseCoffeeCommand>(new { this.CorrelationId, CoffeeType = this.CoffeeTypeRequested, NoTopping = string.IsNullOrWhiteSpace(this.ToppingsRequested) });
            this.State = "Paid";
        }

        public async Task Consume(ConsumeContext<PaymentRefusedEvent> context)
        {
            var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
            await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount });
        }

        public async Task Consume(ConsumeContext<BaseCoffeeFinishedEvent> context)
        {
            if (string.IsNullOrWhiteSpace(this.ToppingsRequested))
            {
                this.State = "Ended";
                return;
            }

            var sendEndpoint = await context.GetSendEndpoint(addToppingsEndpoint);
            await sendEndpoint.Send<AddToppingsCommand>(new { this.CorrelationId, Toppings = this.ToppingsRequested.Split(",").Select(t => Enum.Parse<Topping>(t)) });
            this.State = "BaseCoffeeOK";
        }

        public Task Consume(ConsumeContext<ToppingsAddedEvent> context)
        {
            this.State = "Ended";
            return Task.CompletedTask;
        }
    }
}