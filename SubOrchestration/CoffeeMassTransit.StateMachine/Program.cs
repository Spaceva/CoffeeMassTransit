using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.SubOrchestration.StateMachine;
using CoffeeMassTransit.SubOrchestration.Messages;
using CoffeeMassTransit.Messages;
using Microsoft.Extensions.Configuration;

namespace CoffeeMassTransit.CoffeeMassTransit.SubOrchestration.StateMachine;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddLoggingConfigurationFile()
            .AddDatabaseConfigurationFile()
            .AddRabbitMQConfigurationFile()
            .ConfigureServices(ConfigureServiceCollection)
            .ConfigureSerilog();

    private static void ConfigureServiceCollection(HostBuilderContext hostingContext, IServiceCollection services)
    {
        services.Configure<RabbitMQConfiguration>(hostingContext.Configuration.GetSection("RabbitMQ"));
        services.AddMassTransit(cfgGlobal =>
        {
            cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
            cfgGlobal.AddSagaStateMachine<CoffeeOrderStateMachine, CoffeeOrderState>().DapperRepository(hostingContext.Configuration.GetConnectionString("Local"));
        });
    }

    private static void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
    {
        var rabbitMQConfigurationOption = registrationContext.GetRequiredService<IOptions<RabbitMQConfiguration>>();
        var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

        cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
        {
            cfgRabbitMq.Username(rabbitMQConfiguration.Username);
            cfgRabbitMq.Password(rabbitMQConfiguration.Password);
        });
        cfgBus.ReceiveEndpoint("main-orchestration", e =>
        {
            e.ConfigureSagas(registrationContext);
            e.DiscardSkippedMessages();
            e.DiscardFaultedMessages();
        });
        cfgBus.PurgeOnStartup = true;

        cfgBus.ConfigureOrchestrationMessagesTopology();
        cfgBus.ConfigureSubOrchestrationMessagesTopology();
    }
}
