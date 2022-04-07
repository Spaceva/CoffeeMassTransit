namespace CoffeeMassTransit.DAL.Mongo;

internal class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CoffeeCollectionName { get; set; } = null!;

    public string PaymentCollectionName { get; set; } = null!;
}
