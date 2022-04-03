using CoffeeMassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class HandlePaymentActivity : IExecuteActivity<HandlePaymentArgument>
{
    private readonly ILogger<SubmitOrderActivity> logger;

    public HandlePaymentActivity(ILogger<SubmitOrderActivity> logger)
    {
        this.logger = logger;
    }

    public Task<ExecutionResult> Execute(ExecuteContext<HandlePaymentArgument> context)
    {
        return Task.FromResult(context.Completed<PaymentAccepted>(new { context.CorrelationId }));
    }
}
