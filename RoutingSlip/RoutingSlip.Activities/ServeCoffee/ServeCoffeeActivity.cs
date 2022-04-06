using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class ServeCoffeeActivity : IExecuteActivity<SubmitOrderArguments>
{
    private readonly ILogger<ServeCoffeeActivity> logger;

    public ServeCoffeeActivity(ILogger<ServeCoffeeActivity> logger)
    {
        this.logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<SubmitOrderArguments> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        logger.LogInformation("Coffee {Id} served.", context.TrackingNumber);
        return context.Completed();
    }
}
