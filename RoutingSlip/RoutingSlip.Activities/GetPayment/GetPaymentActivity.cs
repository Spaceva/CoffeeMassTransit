using CoffeeMassTransit.Core.DAL;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class GetPaymentActivity : IActivity<GetPaymentArgument, PaymentAccepted>
{
    private readonly IPaymentRepository paymentRepository;
    private readonly ILogger<SubmitOrderActivity> logger;

    public GetPaymentActivity(IPaymentRepository paymentRepository, ILogger<SubmitOrderActivity> logger)
    {
        this.paymentRepository = paymentRepository;
        this.logger = logger;
    }

    public Task<CompensationResult> Compensate(CompensateContext<PaymentAccepted> context)
    {
        throw new NotImplementedException();
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<GetPaymentArgument> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        paymentRepository.Pay(context.TrackingNumber, "1234567890", "123");
        logger.LogInformation("Payment {Id} done", context.TrackingNumber);
        return context.Completed();
    }
}
