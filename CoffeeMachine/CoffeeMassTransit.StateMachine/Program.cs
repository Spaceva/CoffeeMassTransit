using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.StateMachine;
using MassTransit.RabbitMqTransport;

namespace CoffeeMassTransit.CoffeeMassTransit.StateMachine
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
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
                cfgGlobal.AddSagaStateMachine<CoffeeStateMachine, CoffeeState>().DapperRepository(hostingContext.Configuration.GetConnectionString("Local"));
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
            var repository = registrationContext.GetService<ISagaRepository<CoffeeState>>();
            cfgBus.ReceiveEndpoint("state-machine", e => e.StateMachineSaga(registrationContext.GetService<CoffeeStateMachine>(), repository));
        }
    }
}
