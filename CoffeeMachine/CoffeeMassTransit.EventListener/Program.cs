using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using CoffeeMassTransit.Common;
using MassTransit.RabbitMqTransport;

namespace CoffeeMassTransit.EventListener
{
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
                cfgGlobal.AddConsumersFromNamespaceContaining<OrderSubmittedEventConsumer>();
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
            });
            services.AddHostedService<BusControlService>();
        }

        private static void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
        {
            var rabbitMQConfigurationOption = registrationContext.GetService<IOptions<RabbitMQConfiguration>>();
            var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

            cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
            {
                cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                cfgRabbitMq.Password(rabbitMQConfiguration.Password);
            });

            cfgBus.ConfigureEndpoints(registrationContext, KebabCaseEndpointNameFormatter.Instance);
        }
    }
}
