using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.Web;

public class RequestPaymentCommandConsumer : IConsumer<RequestPaymentCommand>
{
    private readonly PaymentService paymentService;
    private readonly ILogger<RequestPaymentCommandConsumer> logger;

    public RequestPaymentCommandConsumer(PaymentService paymentService, ILogger<RequestPaymentCommandConsumer> logger)
    {
        this.paymentService = paymentService;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<RequestPaymentCommand> context)
    {
        this.logger?.LogInformation("Consuming {CommandName} - {CorrelationId}... Waiting 3s", nameof(RequestPaymentCommand), context.Message.CorrelationId);
        await Task.Delay(TimeSpan.FromSeconds(3), context.CancellationToken);
        this.paymentService.Create(context.CorrelationId!.Value, context.Message.Amount);
    }
}
