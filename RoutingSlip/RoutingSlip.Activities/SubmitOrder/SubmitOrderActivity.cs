using CoffeeMassTransit.Core.DAL;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class SubmitOrderActivity : IExecuteActivity<SubmitOrderArguments>
{
    private readonly IPaymentRepository paymentRepository;
    private readonly ILogger<SubmitOrderActivity> logger;

    public SubmitOrderActivity(IPaymentRepository paymentRepository, ILogger<SubmitOrderActivity> logger)
    {
        this.paymentRepository = paymentRepository;
        this.logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<SubmitOrderArguments> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        this.paymentRepository.Create(context.TrackingNumber, context.Arguments.Amount);
        logger.LogInformation("Order created ({TrackingNumber}, {Amount})", context.TrackingNumber, context.Arguments.Amount);
        return context.Completed();
    }
}
