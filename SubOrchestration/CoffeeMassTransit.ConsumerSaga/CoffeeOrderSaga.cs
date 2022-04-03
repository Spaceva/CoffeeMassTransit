using MassTransit;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using Dapper.Contrib.Extensions;
using CoffeeMassTransit.SubOrchestration.Messages;

namespace CoffeeMassTransit.SubOrchestration.ConsumerSaga;

public class CoffeeOrderSaga : ISaga,
    InitiatedBy<OrderSubmittedEvent>,
    Orchestrates<PaymentAcceptedEvent>,
    Orchestrates<PaymentRefusedEvent>,
    Orchestrates<CoffeeServedEvent>
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string? ToppingsRequested { get; set; }
    public CoffeeType CoffeeTypeRequested { get; set; }
    public float Amount { get; set; }

    public string State { get; private set; } = nameof(CoffeeOrderSagaStates.NotStarted);

    public readonly Uri requestPaymentEndpoint = new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(RequestPaymentCommand))}");
    public readonly Uri createBaseCoffeeEndpoint = new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(CreateBaseCoffeeCommand))}");

    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        this.CustomerName = context.Message.CustomerName;
        this.CoffeeTypeRequested = context.Message.CoffeeType;
        this.ToppingsRequested = string.Join(",", context.Message.Toppings);
        this.Amount = CoffeePriceCalculator.Compute(context.Message.CoffeeType, context.Message.Toppings);
        var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
        await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount }, context.CancellationToken);
        this.State = nameof(CoffeeOrderSagaStates.AwaitingPayment);
    }

    public async Task Consume(ConsumeContext<PaymentAcceptedEvent> context)
    {
        var sendEndpoint = await context.GetSendEndpoint(createBaseCoffeeEndpoint);
        await sendEndpoint.Send<CreateBaseCoffeeCommand>(new { this.CorrelationId, CoffeeType = this.CoffeeTypeRequested, NoTopping = string.IsNullOrWhiteSpace(this.ToppingsRequested) }, context.CancellationToken);
        this.State = nameof(CoffeeOrderSagaStates.Paid);
    }

    public async Task Consume(ConsumeContext<PaymentRefusedEvent> context)
    {
        var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
        await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount }, context.CancellationToken);
    }

    public Task Consume(ConsumeContext<CoffeeServedEvent> context)
    {
        this.State = nameof(CoffeeOrderSagaStates.Ended);
        return Task.CompletedTask;
    }
}
