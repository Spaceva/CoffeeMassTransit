using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.DemoCommon;

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
            .ConfigureServices(ConfigureServiceCollection)
            .ConfigureSerilog();

    private static void ConfigureServiceCollection(HostBuilderContext hostingContext, IServiceCollection services)
    {
        services.Configure<RabbitMQConfiguration>(hostingContext.Configuration.GetSection("RabbitMQ"));
        services.AddMassTransit(cfgGlobal =>
        {
            cfgGlobal.AddConsumersFromNamespaceContaining<InformationRequestConsumer>();
            cfgGlobal.AddConsumersFromNamespaceContaining<PublicMessageConsumer>();
            cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
        });
        services.AddHostedService<PublicMessageSpammer>();
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

        cfgBus.ReceiveEndpoint("serviceB", cfgEndpoint =>
        {
            cfgEndpoint.ConfigureConsumers(registrationContext);
            cfgEndpoint.UseMessageRetry(cfgRetry =>
            {
                cfgRetry.Interval(2, TimeSpan.FromMilliseconds(500));
            });
            cfgEndpoint.PurgeOnStartup = true;
        });
    }
}
