using CoffeeMassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace CoffeeMassTransit.DAL.Mongo;
internal class PaymentConnectionFactory : ConnectionFactory<Payment>
{
    public PaymentConnectionFactory(IOptions<MongoDbSettings> options) : base(options)
    {
    }

    protected override string SelectCollectionName(MongoDbSettings mongoDbSettings)
     => mongoDbSettings.PaymentCollectionName;
}
