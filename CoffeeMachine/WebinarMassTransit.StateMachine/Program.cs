using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using WebinarMassTransit.Common;
using WebinarMassTransit.StateMachine;

namespace WebinarMassTransit.WebinarMassTransit.StateMachine
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
                cfgGlobal.AddBus(ConfigureRabbitMQ);
                cfgGlobal.AddSagaStateMachine<CoffeeStateMachine, CoffeeState>().InMemoryRepository();
            });
            services.AddHostedService<BusControlService>();
        }

        private static IBusControl ConfigureRabbitMQ(IRegistrationContext<IServiceProvider> registrationContext)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfgBus =>
            {
                var rabbitMQConfigurationOption = registrationContext.Container.GetService<IOptions<RabbitMQConfiguration>>();
                var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

                cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
                {
                    cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                    cfgRabbitMq.Password(rabbitMQConfiguration.Password);
                });
                var repository = registrationContext.Container.GetService<ISagaRepository<CoffeeState>>();
                cfgBus.ReceiveEndpoint("state-machine", e => e.StateMachineSaga(registrationContext.Container.GetService<CoffeeStateMachine>(), repository));
            });
        }
    }
}
