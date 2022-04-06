using CoffeeMassTransit.Contracts;
using MassTransit;

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
        var trackingNumber = NewId.NextGuid();
        var builder = new RoutingSlipBuilder(trackingNumber);

        var toppingsRequested = Array.Empty<Topping>();
        var coffeeTypeRequested = CoffeeType.Espresso;

        builder.AddActivity("Création de la commande", new Uri("queue:step-1"), new
        {
            CustomerName = "Test",
            Toppings = toppingsRequested,
            CoffeeType = coffeeTypeRequested,
            Amount = 10,
        });
        builder.AddActivity("Paiement facture", new Uri("queue:step-2"), new
        {
            Amount = 10,
        });
        builder.AddActivity("Création du café", new Uri("queue:step-3"), new
        {
            CoffeeType = coffeeTypeRequested,
        });
        if (toppingsRequested.Length > 0)
        {
            builder.AddActivity("Ajout des topings", new Uri("queue:step-3-bis"), new
            {
                Toppings = toppingsRequested,
            });
        }

        builder.AddActivity("Livraison du café", new Uri("queue:step-4"));
        var orchestration = builder.Build();
        logger.LogInformation("Started Orchestration {TrackingNumber}", trackingNumber);
        await bus.Execute(orchestration);
    }
}
