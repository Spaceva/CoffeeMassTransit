using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMassTransit.Core.DAL;
public static partial class ServiceCollectionExtensions
{
    public static void AddRepositoriesInMemory(this IServiceCollection services)
    {
        services.AddSingleton<ICoffeeRepository, CoffeeInMemoryRepository>();
        services.AddSingleton<IPaymentRepository, PaymentInMemoryRepository>();
    }
}
