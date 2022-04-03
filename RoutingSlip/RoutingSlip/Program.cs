using Serilog;
using MassTransit;
using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.Core;
using Microsoft.Extensions.Options;
using RoutingSlip;
using RoutingSlip.Activities;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration, builder.Environment);
ConfigureServices(builder.Services, builder.Configuration);
builder.Host.UseSerilog(ConfigureLogging);

var application = builder.Build();

ConfigureMiddleware(application, application.Environment);
ConfigureEndpoints(application);

application.Run();

static void ConfigureLogging(HostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
    Directory.CreateDirectory("App_Data/Logs");
    Serilog.Debugging.SelfLog.Enable(new StreamWriter("App_Data/Logs/serilog-failures.txt", true));
}

static void ConfigureConfiguration(IConfigurationBuilder configuration, IWebHostEnvironment environment)
{
    configuration.AddJsonFile("logging.json", optional: true, reloadOnChange: true)
                 .AddJsonFile("rabbitmq.json", optional: true, reloadOnChange: true)
                 .AddJsonFile("database.json", optional: true, reloadOnChange: true);
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllersWithViews();
    services.Configure<RabbitMQConfiguration>(configuration.GetSection("RabbitMQ"));
    services.AddSingleton<SqlConnectionFactory>(new LocalSqlConnectionFactory(configuration.GetConnectionString("Local")));
    services.AddMassTransit(cfgGlobal =>
    {
        cfgGlobal.AddActivitiesFromNamespaceContaining<SubmitOrderActivity>();
        cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
    });
    services.AddSingleton<IPaymentRepository, PaymentDapperRepository>();
    services.AddSingleton<ICoffeeRepository, CoffeeDapperRepository>();
    services.AddControllersWithViews();
    services.AddHealthChecks();
    services.AddHostedService<TestActivitiesOrchestrationBackgroundService>();
}

static void ConfigureMiddleware(IApplicationBuilder application, IWebHostEnvironment environment)
{
    if (environment.IsDevelopment())
    {
        application.UseDeveloperExceptionPage();
    }
    else
    {
        application.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        application.UseHsts();
    }

    application.UseHttpsRedirection();
    application.UseStaticFiles();

    application.UseRouting();

    application.UseAuthorization();
}

static void ConfigureEndpoints(IEndpointRouteBuilder application)
{
    application.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    application.MapHealthChecks("/health");
}

static void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
{
    var rabbitMQConfigurationOption = registrationContext.GetRequiredService<IOptions<RabbitMQConfiguration>>();
    var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

    cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
    {
        cfgRabbitMq.Username(rabbitMQConfiguration.Username);
        cfgRabbitMq.Password(rabbitMQConfiguration.Password);
    });

    cfgBus.ReceiveEndpoint("step-1", cfgEndpoint =>
    {
        cfgEndpoint.ConfigureActivityExecute(registrationContext, typeof(SubmitOrderActivity), new Uri("queue:step-1-c"));
    });

    cfgBus.ReceiveEndpoint("step-1-c", cfgEndpoint =>
    {
        cfgEndpoint.ConfigureActivityCompensate(registrationContext, typeof(SubmitOrderActivity));
    });

    cfgBus.ReceiveEndpoint("step-2", cfgEndpoint =>
    {
        cfgEndpoint.ConfigureExecuteActivity(registrationContext, typeof(HandlePaymentActivity));
    });
}