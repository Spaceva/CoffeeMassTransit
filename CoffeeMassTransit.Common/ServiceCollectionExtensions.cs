﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using MassTransit;
using Microsoft.Extensions.Options;

namespace CoffeeMassTransit.Common;
public static class ServiceCollectionExtensions
{
    public static void ConfigureAzureServiceBus(this IServiceCollection services)
    => services.AddOptions<AzureServiceBusConfiguration>().Configure<IConfiguration>((settings, configuration) => configuration.Bind("AzureServiceBus", settings));

    public static IServiceCollection ConfigureAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<AzureServiceBusConfiguration>(configuration.GetSection("AzureServiceBus"));


    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? configureMassTransit = null, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureBus = null)
    {
        services.Configure<RabbitMQConfiguration>(configuration.GetSection("RabbitMQ"));
        return services.AddMassTransit(cfgMassTransit =>
        {
            cfgMassTransit.UsingRabbitMq((registrationContext, cfgBus) =>
            {
                var rabbitMQConfigurationOption = registrationContext.GetRequiredService<IOptions<RabbitMQConfiguration>>();
                var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;
                cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
                {
                    cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                    cfgRabbitMq.Password(rabbitMQConfiguration.Password);
                });

                configureBus?.Invoke(registrationContext, cfgBus);
            });

            configureMassTransit?.Invoke(cfgMassTransit);
        });
    }

    public static IServiceCollection AddMassTransitWithAzureServiceBus(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? configureMassTransit = null, Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? configureBus = null)
    {
        services.ConfigureAzureServiceBus(configuration);
        return services.AddMassTransit(cfgMassTransit =>
        {
            cfgMassTransit.UsingAzureServiceBus((registrationContext, cfgBus) =>
            {
                var azureServiceBusConfigurationOptions = registrationContext.GetService<IOptions<AzureServiceBusConfiguration>>();
                var azureServiceBusConfiguration = azureServiceBusConfigurationOptions!.Value;
                cfgBus.Host(azureServiceBusConfiguration.ConnectionString, cfgHost =>
                {
                    cfgHost.TransportType = Azure.Messaging.ServiceBus.ServiceBusTransportType.AmqpWebSockets;
                });

                configureBus?.Invoke(registrationContext, cfgBus);
            });

            configureMassTransit?.Invoke(cfgMassTransit);
        });
    }

}
