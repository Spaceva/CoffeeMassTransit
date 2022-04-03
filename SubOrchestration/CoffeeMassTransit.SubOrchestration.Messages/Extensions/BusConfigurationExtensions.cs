using MassTransit;

namespace CoffeeMassTransit.SubOrchestration.Messages;
public static class BusConfigurationExtensions
{
    public static void ConfigureSubOrchestrationMessagesTopology(this IRabbitMqBusFactoryConfigurator cfgBus)
    {
        cfgBus.Message<CoffeeMadeEvent>(x =>
        {
            x.SetEntityName($"sub-orchestration-events");
        });

        cfgBus.Message<Fault<CoffeeMadeEvent>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<CoffeeServedEvent>(x =>
        {
            x.SetEntityName($"sub-orchestration-events");
        });

        cfgBus.Message<Fault<CoffeeServedEvent>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<CoffeeTankCheckedEvent>(x =>
        {
            x.SetEntityName($"sub-orchestration-events");
        });

        cfgBus.Message<Fault<CoffeeTankCheckedEvent>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<MilkTankCheckedEvent>(x =>
        {
            x.SetEntityName($"sub-orchestration-events");
        });

        cfgBus.Message<Fault<MilkTankCheckedEvent>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<CheckCoffeeTankCommand>(x =>
        {
            x.SetEntityName($"sub-orchestration-commands");
        });

        cfgBus.Message<CheckMilkTankCommand>(x =>
        {
            x.SetEntityName($"sub-orchestration-commands");
        });

        cfgBus.Message<CreateCoffeeCommand>(x =>
        {
            x.SetEntityName($"sub-orchestration-commands");
        });

        cfgBus.Message<MakeCoffeeCommand>(x =>
        {
            x.SetEntityName($"sub-orchestration-commands");
        });

        cfgBus.Message<Fault<CheckCoffeeTankCommand>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<Fault<CheckMilkTankCommand>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<Fault<CreateCoffeeCommand>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });

        cfgBus.Message<Fault<MakeCoffeeCommand>>(x =>
        {
            x.SetEntityName($"sub-orchestration-errors");
        });
    }
}
