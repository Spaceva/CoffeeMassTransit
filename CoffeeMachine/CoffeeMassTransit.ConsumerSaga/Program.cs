using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;

namespace CoffeeMassTransit.CoffeeMassTransit.ConsumerSaga
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
                cfgGlobal.AddSaga<CoffeeMachineSaga>().InMemoryRepository();
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
                cfgBus.ConfigureEndpoints(registrationContext);
            });
        }
    }
}
