using CoffeeMassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class SubmitOrderActivity : IActivity<SubmitOrderArguments, OrderSubmitted>
{
    private readonly ILogger<SubmitOrderActivity> logger;

    public SubmitOrderActivity(ILogger<SubmitOrderActivity> logger)
    {
        this.logger = logger;
    }

    public Task<CompensationResult> Compensate(CompensateContext<OrderSubmitted> context)
    {
        this.logger.LogInformation("Error happened : {CustomerName}", context.Log.CustomerName);
        return Task.FromResult(context.Compensated());
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<SubmitOrderArguments> context)
    {
        try
        {
            return context.Completed<OrderSubmitted>(new { context.CorrelationId, CoffeeType = context.Arguments.CoffeeTypeRequested, Toppings = context.Arguments.ToppingsRequested, CustomerName = context.Arguments.CustomerName });
        }
        catch (Exception ex)
        {
            return context.Faulted(ex);
        }
    }
}
