using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.DemoCommon;
using MassTransit.RabbitMqTransport;

namespace CoffeeMassTransit.DemoServiceA
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
                .AddRabbitMQConfigurationFile()
                .ConfigureServices(ConfigureServiceCollection)
                .ConfigureSerilog();

        private static void ConfigureServiceCollection(HostBuilderContext hostingContext, IServiceCollection services)
        {
            services.Configure<RabbitMQConfiguration>(hostingContext.Configuration.GetSection("RabbitMQ"));
            services.AddMassTransit(cfgGlobal =>
            {
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
                cfgGlobal.AddConsumersFromNamespaceContaining<PublicMessageConsumer>();
                cfgGlobal.AddConsumersFromNamespaceContaining<InformationResponseConsumer>();
                cfgGlobal.AddRequestClient<StatusCheck>();
            });
            services.AddHostedService<BusControlService>();
            services.AddHostedService<InformationRequestService>();
            services.AddHostedService<StatusChecker>();
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

            cfgBus.ReceiveEndpoint("serviceA", cfgEndpoint =>
            {
                cfgEndpoint.ConfigureConsumers(registrationContext);
                cfgEndpoint.PurgeOnStartup = true;
            });
        }
    }
}
