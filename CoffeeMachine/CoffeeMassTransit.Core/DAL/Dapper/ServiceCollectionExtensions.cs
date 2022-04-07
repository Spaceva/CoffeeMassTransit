using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMassTransit.Core.DAL;
public static partial class ServiceCollectionExtensions
{
    public static void AddRepositoriesInSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SqlConnectionFactory>(new LocalSqlConnectionFactory(configuration.GetConnectionString("Local")));
        services.AddSingleton<ICoffeeRepository, CoffeeDapperRepository>();
        services.AddSingleton<IPaymentRepository, PaymentDapperRepository>();
    }
}
