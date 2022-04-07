using CoffeeMassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace CoffeeMassTransit.DAL.Mongo;
internal class CoffeeConnectionFactory : ConnectionFactory<Coffee>
{
    public CoffeeConnectionFactory(IOptions<MongoDbSettings> options) : base(options)
    {
    }

    protected override string SelectCollectionName(MongoDbSettings mongoDbSettings)
     => mongoDbSettings.CoffeeCollectionName;
}
