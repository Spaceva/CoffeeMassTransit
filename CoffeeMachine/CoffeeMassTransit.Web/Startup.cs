using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using CoffeeMassTransit.Common;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.Messages;
using MassTransit.RabbitMqTransport;

namespace CoffeeMassTransit.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<RabbitMQConfiguration>(Configuration.GetSection("RabbitMQ"));
            services.AddSingleton<SqlConnectionFactory>(new LocalSqlConnectionFactory(Configuration.GetConnectionString("Local")));
            services.AddMassTransit(cfgGlobal =>
            {
                cfgGlobal.AddConsumer<RequestPaymentCommandConsumer>();
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMQ);
            });
            services.AddHostedService<BusControlService>();
            services.AddTransient<OrderService>();
            services.AddTransient<PaymentService>();
            services.AddTransient<CoffeeService>();
            services.AddSingleton<IPaymentRepository, PaymentDapperRepository>();
            services.AddSingleton<ICoffeeRepository, CoffeeDapperRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureRabbitMQ(IBusRegistrationContext registrationContext, IRabbitMqBusFactoryConfigurator cfgBus)
        {
            var rabbitMQConfigurationOption = registrationContext.GetService<IOptions<RabbitMQConfiguration>>();
            var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

            cfgBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
            {
                cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                cfgRabbitMq.Password(rabbitMQConfiguration.Password);
            });

            cfgBus.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(RequestPaymentCommand)),
                                cfgEndpoint =>
                                {
                                    cfgEndpoint.ConfigureConsumer<RequestPaymentCommandConsumer>(registrationContext);
                                    cfgEndpoint.UseRetry(cfgRetry =>
                                    {
                                        cfgRetry.Interval(3, TimeSpan.FromSeconds(5));
                                    });
                                });
        }
    }
}
