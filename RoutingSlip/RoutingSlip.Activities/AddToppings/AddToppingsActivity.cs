using CoffeeMassTransit.Core.DAL;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class AddToppingsActivity : IExecuteActivity<AddToppingsArguments>
{
    private readonly ICoffeeRepository coffeeRepository;
    private readonly ILogger<AddToppingsActivity> logger;

    public AddToppingsActivity(ICoffeeRepository coffeeRepository, ILogger<AddToppingsActivity> logger)
    {
        this.coffeeRepository = coffeeRepository;
        this.logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<AddToppingsArguments> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        coffeeRepository.AddToppings(context.TrackingNumber, context.Arguments.Toppings);
        logger.LogInformation("Toppings for coffee {Id} added", context.TrackingNumber);
        return context.Completed();
    }
}
