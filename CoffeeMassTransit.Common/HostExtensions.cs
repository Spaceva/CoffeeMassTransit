using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace CoffeeMassTransit.Common;

public static class HostExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        return builder.UseSerilog(ConfigureLogging);
    }

    public static IHostBuilder AddLoggingConfigurationFile(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(config => config.AddJsonFile("logging.json", optional: true, reloadOnChange: true));
    }

    public static IHostBuilder AddRabbitMQConfigurationFile(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(config => config.AddJsonFile("rabbitmq.json", optional: true, reloadOnChange: true));
    }

    public static IHostBuilder AddAzureServiceBusConfigurationFile(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(config => config.AddJsonFile("azure-service-bus.json", optional: true, reloadOnChange: true));
    }

    public static IHostBuilder AddDatabaseConfigurationFile(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(config => config.AddJsonFile("database.json", optional: true, reloadOnChange: true));
    }

    private static void ConfigureLogging(HostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        Serilog.Debugging.SelfLog.Enable(Console.Error);
    }
}
