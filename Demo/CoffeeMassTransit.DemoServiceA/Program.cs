using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.DemoCommon;
using CoffeeMassTransit.DemoCommon.Extensions;

namespace CoffeeMassTransit.DemoServiceA;

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
        /*services.AddMassTransitWithRabbitMQ(hostingContext.Configuration, cfgGlobal =>
        {
            cfgGlobal.AddConsumersFromNamespaceContaining<PublicMessageConsumer>();
            cfgGlobal.AddConsumersFromNamespaceContaining<InformationResponseConsumer>();
        }, ConfigureRabbitMQ);*/
        services.AddMassTransitWithAzureServiceBus(hostingContext.Configuration, cfgGlobal =>
        {
            cfgGlobal.AddConsumersFromNamespaceContaining<PublicMessageConsumer>();
            cfgGlobal.AddConsumersFromNamespaceContaining<InformationResponseConsumer>();
        }, ConfigureAzureServiceBus);
        services.AddHostedService<InformationRequester>();
        // services.AddHostedService<StatusChecker>();
    }

    private static void ConfigureAzureServiceBus(IBusRegistrationContext registrationContext, IServiceBusBusFactoryConfigurator cfgBus)
    {
        cfgBus.ReceiveEndpoint("serviceA", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureConsumers(registrationContext);
        });

        cfgBus.ConfigureMessagesTopology();
    }

    private static void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
    {
        cfgBus.ReceiveEndpoint("serviceA", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureConsumers(registrationContext);
            cfgEndpoint.PurgeOnStartup = true;
        });

        cfgBus.ConfigureMessagesTopology();
    }
}
