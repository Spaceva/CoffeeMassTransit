using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoffeeMassTransit.DAL.Mongo;

internal abstract class ConnectionFactory<TEntity>
{
    protected MongoDbSettings MongoDbSettings { get; }

    public ConnectionFactory(IOptions<MongoDbSettings> options)
    {
        MongoDbSettings = options.Value;
    }

    public IMongoCollection<TEntity> GetCollection()
    {
        var mongoClient = new MongoClient(MongoDbSettings.ConnectionString);

        var database = mongoClient.GetDatabase(MongoDbSettings.DatabaseName);

        return database.GetCollection<TEntity>(SelectCollectionName(this.MongoDbSettings));
    }

    protected abstract string SelectCollectionName(MongoDbSettings mongoDbSettings);
}
