using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.DemoCommon;
using CoffeeMassTransit.DemoCommon.Extensions;

namespace CoffeeMassTransit.DemoServiceB;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddLoggingConfigurationFile()
            .AddRabbitMQConfigurationFile()
            .AddAzureServiceBusConfigurationFile()
            .ConfigureServices(ConfigureServiceCollection)
            .ConfigureSerilog();

    private static void ConfigureServiceCollection(HostBuilderContext hostingContext, IServiceCollection services)
    {
        // => services.AddMassTransitWithRabbitMQ(hostingContext.Configuration, RegisterConsumers, ConfigureRabbitMQ);
        services.AddMassTransitWithAzureServiceBus(hostingContext.Configuration, RegisterConsumers, ConfigureAzureServiceBus);
        services.AddHostedService<PublicMessageSpammer>();
    }

    private static void RegisterConsumers(IBusRegistrationConfigurator cfgGlobal)
    {
        cfgGlobal.AddConsumer<PublicMessageConsumer>().Endpoint(e =>
        {
            e.Name = $"Name-i-picked-for-{nameof(PublicMessageConsumer)}";
        });
        cfgGlobal.AddConsumer<PublicMessageReceivedConsumer>().Endpoint(e =>
        {
            e.Name = $"Name-i-picked-for-{nameof(PublicMessageReceivedConsumer)}";
        });
        cfgGlobal.AddConsumer<FaultedInformationRequestConsumer>().Endpoint(e =>
        {
            e.Name = $"Name-i-picked-for-{nameof(FaultedInformationRequestConsumer)}";
        });
        cfgGlobal.AddConsumer<InformationRequestConsumer>().Endpoint(e =>
        {
            e.Name = $"Name-i-picked-for-{nameof(InformationRequestConsumer)}";
        });
        cfgGlobal.AddConsumer<StatusCheckConsumer>().Endpoint(e =>
        {
            e.Name = $"Name-i-picked-for-{nameof(StatusCheckConsumer)}";
        });
    }
    private static void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
    {
        cfgBus.ReceiveEndpoint("serviceB", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureConsumers(registrationContext);
            cfgEndpoint.UseMessageRetry(cfgRetry =>
            {
                cfgRetry.Interval(2, TimeSpan.FromMilliseconds(500));
            });
            cfgEndpoint.PurgeOnStartup = true;
        });

        cfgBus.ConfigureMessagesTopology();
    }
    
    private static void ConfigureAzureServiceBus(IBusRegistrationContext registrationContext, IServiceBusBusFactoryConfigurator cfgBus)
    {
        cfgBus.ReceiveEndpoint("serviceB", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureConsumers(registrationContext);
            /*cfgEndpoint.UseMessageRetry(cfgRetry =>
            {
                cfgRetry.Interval(2, TimeSpan.FromMilliseconds(500));
            });*/
        });

        cfgBus.ConfigureMessagesTopology();
    }
}
