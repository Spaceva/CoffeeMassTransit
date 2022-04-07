using CoffeeMassTransit.Core.DAL;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RoutingSlip.Activities;
public class MakeCoffeeActivity : IExecuteActivity<MakeCoffeeArguments>
{
    private readonly ICoffeeRepository coffeeRepository;
    private readonly ILogger<MakeCoffeeActivity> logger;

    public MakeCoffeeActivity(ICoffeeRepository coffeeRepository, ILogger<MakeCoffeeActivity> logger)
    {
        this.coffeeRepository = coffeeRepository;
        this.logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<MakeCoffeeArguments> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        coffeeRepository.Create(context.TrackingNumber, context.TrackingNumber, context.Arguments.CoffeeType, context.Arguments.NoToppings);
        logger.LogInformation("Coffee for Order {Id} made", context.TrackingNumber);
        return context.Completed();
    }
}
