using MassTransit;

namespace CoffeeMassTransit.DemoCommon.Extensions;
public static class BusConfigurationExtensions
{
    public static void ConfigureMessagesTopology(this IBusFactoryConfigurator cfgBus)
    {
        cfgBus.Message<PublicMessage>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(PublicMessage)}");
        });

        cfgBus.Message<Fault<PublicMessage>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(PublicMessage)}");
        });

        cfgBus.Message<PublicMessageReceived>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(PublicMessageReceived)}");
        });

        cfgBus.Message<Fault<PublicMessageReceived>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(PublicMessageReceived)}");
        });

        cfgBus.Message<AccessDenied>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(AccessDenied)}");
        });

        cfgBus.Message<Fault<AccessDenied>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(AccessDenied)}");
        });

        cfgBus.Message<InformationRequest>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(InformationRequest)}");
        });

        cfgBus.Message<Fault<InformationRequest>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(InformationRequest)}");
        });

        cfgBus.Message<InformationResponse>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(AccessDenied)}");
        });

        cfgBus.Message<Fault<InformationResponse>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(InformationResponse)}");
        });

        cfgBus.Message<InformationRequest>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(InformationRequest)}");
        });

        cfgBus.Message<Fault<InformationRequest>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With-{nameof(InformationRequest)}");
        });

        cfgBus.Message<StatusCheck>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-{nameof(StatusCheck)}");
        });

        cfgBus.Message<Fault<StatusCheck>>(x =>
        {
            x.SetEntityName($"Name-I-Picked-For-Error-With{nameof(StatusCheck)}");
        });
    }
}
