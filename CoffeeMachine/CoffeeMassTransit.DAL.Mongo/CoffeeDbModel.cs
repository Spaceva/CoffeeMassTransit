using CoffeeMassTransit.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoffeeMassTransit.DAL.Mongo;

internal class CoffeeDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = null!;

    public Guid OrderId { get; init; }

    public CoffeeType Type { get; init; }

    public string? Toppings { get; init; }

    public bool Done { get; init; }
}
