using MassTransit;

namespace CoffeeMassTransit.Messages;
public static class BusConfigurationExtensions
{
    public static void ConfigureOrchestrationMessagesTopology(this IRabbitMqBusFactoryConfigurator cfgBus)
    {
        cfgBus.Message<OrderSubmittedEvent>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<Fault<OrderSubmittedEvent>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<PaymentAcceptedEvent>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<Fault<PaymentAcceptedEvent>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<PaymentRefusedEvent>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<Fault<PaymentRefusedEvent>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<BaseCoffeeFinishedEvent>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<Fault<BaseCoffeeFinishedEvent>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<ToppingsAddedEvent>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<Fault<ToppingsAddedEvent>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<AddToppingsCommand>(x =>
        {
            x.SetEntityName($"commands");
        });

        cfgBus.Message<CreateBaseCoffeeCommand>(x =>
        {
            x.SetEntityName($"commands");
        });

        cfgBus.Message<RequestPaymentCommand>(x =>
        {
            x.SetEntityName($"commands");
        });

        cfgBus.Message<Fault<AddToppingsCommand>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<Fault<CreateBaseCoffeeCommand>>(x =>
        {
            x.SetEntityName($"errors");
        });

        cfgBus.Message<Fault<RequestPaymentCommand>>(x =>
        {
            x.SetEntityName($"errors");
        });
    }
}
