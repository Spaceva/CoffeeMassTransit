﻿using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using WebinarMassTransit.Common;
using WebinarMassTransit.Core;
using WebinarMassTransit.Core.DAL;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.CoffeeMachine
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
            services.AddSingleton<SqlConnectionFactory>(new LocalSqlConnectionFactory(hostingContext.Configuration.GetConnectionString("Local")));
            services.AddSingleton<ICoffeeRepository, CoffeeDapperRepository>();
            services.AddMassTransit(cfgGlobal =>
            {
                cfgGlobal.AddConsumer<CreateBaseCoffeeCommandConsumer>();
                cfgGlobal.AddBus(ConfigureRabbitMQ);
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

                cfgBus.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(CreateBaseCoffeeCommand)),
                     cfgEndpoint =>
                     {
                         cfgEndpoint.ConfigureConsumer<CreateBaseCoffeeCommandConsumer>(registrationContext);
                         cfgEndpoint.UseRetry(cfgRetry =>
                         {
                             cfgRetry.Interval(3, TimeSpan.FromSeconds(5));
                         });
                     });
            });
        }
    }
}
