using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.SubOrchestration.Messages;
using CoffeeMassTransit.Messages;
using CoffeeMassTransit.DAL.Mongo;

namespace CoffeeMassTransit.SubOrchestration.Consumers;

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
        services.AddRepositoriesInMongoDB(hostingContext.Configuration);
        services.AddMassTransit(cfgGlobal =>
        {
            cfgGlobal.AddConsumersFromNamespaceContaining<CheckMilkTankCommandConsumer>();
            cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
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

        cfgBus.ReceiveEndpoint("sub-orchestration-commands",
             cfgEndpoint =>
             {
                 cfgEndpoint.ConfigureConsumers(registrationContext);
                 cfgEndpoint.UseRetry(cfgRetry =>
                 {
                     cfgRetry.Interval(3, TimeSpan.FromSeconds(5));
                 });
                 cfgEndpoint.PurgeOnStartup = true;
                 cfgEndpoint.DiscardSkippedMessages();
                 cfgEndpoint.DiscardFaultedMessages();
             });

        cfgBus.ConfigureOrchestrationMessagesTopology();
        cfgBus.ConfigureSubOrchestrationMessagesTopology();
    }
}
