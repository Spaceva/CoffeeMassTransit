using CoffeeMassTransit.Contracts;
using MassTransit;
using RoutingSlip.Activities;

namespace RoutingSlip;

public class TestActivitiesOrchestrationBackgroundService : BackgroundService
{
    private readonly IBus bus;
    private readonly ILogger<TestActivitiesOrchestrationBackgroundService> logger;

    public TestActivitiesOrchestrationBackgroundService(IBus bus, ILogger<TestActivitiesOrchestrationBackgroundService> logger)
    {
        this.bus = bus;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddActivity("Step 1", new Uri("queue:step-1"), new
        {
            CustomerName = "Test",
            ToppingsRequested = "None",
            CoffeeTypeRequested = CoffeeType.Espresso,
            Amount = 10,
        });
        builder.AddActivity("Step 2", new Uri("queue:step-2"), new
        {
            Amount = 10,
        });
        var orchestration = builder.Build();
        await bus.Execute(orchestration);
        logger.LogInformation("Finished !");
    }
}
