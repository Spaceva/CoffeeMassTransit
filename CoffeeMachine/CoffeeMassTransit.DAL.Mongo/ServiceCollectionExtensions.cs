using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMassTransit.DAL.Mongo;
public static class ServiceCollectionExtensions
{
    public static void AddRepositoriesInMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));
        services.AddSingleton<ConnectionFactory<Payment>, PaymentConnectionFactory>();
        services.AddSingleton<ConnectionFactory<Coffee>, CoffeeConnectionFactory>();
        services.AddSingleton<ICoffeeRepository, CoffeeMongoRepository>();
        services.AddSingleton<IPaymentRepository, PaymentMongoRepository>();
    }
}
