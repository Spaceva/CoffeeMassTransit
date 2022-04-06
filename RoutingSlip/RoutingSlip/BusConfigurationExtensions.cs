using MassTransit;
using MassTransit.Courier.Contracts;
using RoutingSlip.Activities;

namespace RoutingSlip;

public static class BusConfigurationExtensions
{
    public static void ConfigureCoffeeActivitiesEndpoint(this IRabbitMqBusFactoryConfigurator cfgBus, IBusRegistrationContext registrationContext)
    {

        cfgBus.ReceiveEndpoint("step-1", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(SubmitOrderActivity));
        });

        cfgBus.ReceiveEndpoint("step-2", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(GetPaymentActivity));
        });

        cfgBus.ReceiveEndpoint("step-3", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(MakeCoffeeActivity));
        });

        cfgBus.ReceiveEndpoint("step-3-bis", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(AddToppingsActivity));
        });

        cfgBus.ReceiveEndpoint("step-4", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(ServeCoffeeActivity));
        });
    }

    public static void ConfigureRoutingSlipMessagesTopology(this IRabbitMqBusFactoryConfigurator cfgBus)
    {
        cfgBus.Message<RoutingSlipCompleted>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipFaulted>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipCompensationFailed>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipActivityCompleted>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipActivityFaulted>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipActivityCompensated>(x =>
        {
            x.SetEntityName($"events");
        });

        cfgBus.Message<RoutingSlipActivityCompensationFailed>(x =>
        {
            x.SetEntityName($"events");
        });
    }
}
