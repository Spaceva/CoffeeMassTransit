﻿using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using Dapper.Contrib.Extensions;

namespace CoffeeMassTransit.CoffeeMassTransit.ConsumerSaga;

public class CoffeeMachineSaga : ISaga,
    InitiatedBy<OrderSubmittedEvent>,
    Orchestrates<PaymentAcceptedEvent>,
    Orchestrates<PaymentRefusedEvent>,
    Orchestrates<BaseCoffeeFinishedEvent>,
    Orchestrates<ToppingsAddedEvent>
{
    [ExplicitKey] 
    public Guid CorrelationId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string? ToppingsRequested { get; set; }
    public CoffeeType CoffeeTypeRequested { get; set; }
    public float Amount { get; set; }

    public string State { get; private set; } = nameof(CoffeeMachineSagaStates.NotStarted);

    public readonly Uri requestPaymentEndpoint = new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(RequestPaymentCommand))}");
    public readonly Uri createBaseCoffeeEndpoint = new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(CreateBaseCoffeeCommand))}");
    public readonly Uri addToppingsEndpoint = new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(AddToppingsCommand))}");
   
    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        this.CustomerName = context.Message.CustomerName;
        this.CoffeeTypeRequested = context.Message.CoffeeType;
        this.ToppingsRequested = string.Join(",", context.Message.Toppings);
        this.Amount = CoffeePriceCalculator.Compute(context.Message.CoffeeType, context.Message.Toppings);
        var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
        await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount }, context.CancellationToken);
        this.State = nameof(CoffeeMachineSagaStates.AwaitingPayment);

    }

    public async Task Consume(ConsumeContext<PaymentAcceptedEvent> context)
    {
        var sendEndpoint = await context.GetSendEndpoint(createBaseCoffeeEndpoint);
        await sendEndpoint.Send<CreateBaseCoffeeCommand>(new { this.CorrelationId, CoffeeType = this.CoffeeTypeRequested, NoTopping = string.IsNullOrWhiteSpace(this.ToppingsRequested) }, context.CancellationToken);
        this.State = nameof(CoffeeMachineSagaStates.Paid);
    }

    public async Task Consume(ConsumeContext<PaymentRefusedEvent> context)
    {
        var sendEndpoint = await context.GetSendEndpoint(requestPaymentEndpoint);
        await sendEndpoint.Send<RequestPaymentCommand>(new { this.CorrelationId, this.Amount }, context.CancellationToken);
    }

    public async Task Consume(ConsumeContext<BaseCoffeeFinishedEvent> context)
    {
        if (string.IsNullOrWhiteSpace(this.ToppingsRequested))
        {
            this.State = nameof(CoffeeMachineSagaStates.Ended);
            return;
        }

        var sendEndpoint = await context.GetSendEndpoint(addToppingsEndpoint);
        await sendEndpoint.Send<AddToppingsCommand>(new { this.CorrelationId, Toppings = this.ToppingsRequested.Split(",").Select(t => Enum.Parse<Topping>(t)) }, context.CancellationToken);
        this.State = nameof(CoffeeMachineSagaStates.BaseCoffeeOK);
    }

    public Task Consume(ConsumeContext<ToppingsAddedEvent> context)
    {
        this.State = nameof(CoffeeMachineSagaStates.Ended);
        return Task.CompletedTask;
    }
}
