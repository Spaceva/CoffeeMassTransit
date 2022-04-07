using CoffeeMassTransit.Contracts;
using System.Text.Json;
using MongoDB.Driver;
using CoffeeMassTransit.Core.DAL;

namespace CoffeeMassTransit.DAL.Mongo;

internal class CoffeeMongoRepository : ICoffeeRepository
{
    private readonly IMongoCollection<Coffee> coffees;

    public CoffeeMongoRepository(ConnectionFactory<Coffee> connectionFactory)
    {
        this.coffees = connectionFactory.GetCollection();
    }

    public async void Create(Guid coffeeId, Guid orderId, CoffeeType coffeeType, bool noTopping)
    {
        await coffees.InsertOneAsync(new Coffee
        {
            Id = coffeeId,
            OrderId = orderId,
            Done = noTopping,
            Type = coffeeType,
        });
    }

    public void AddToppings(Guid coffeeId, IReadOnlyCollection<Topping> toppings)
    {
        var jsonFind = (FilterDefinition<Coffee>)JsonSerializer.Serialize(new { Id = coffeeId });

        var jsonUpdate = (UpdateDefinition<Coffee>)JsonSerializer.Serialize(new
        {
            Toppings = toppings,
            Done = true
        });

        coffees.FindOneAndUpdate(jsonFind, jsonUpdate);
    }

    public Coffee Get(Guid coffeeId)
    {
        var jsonFind = (FilterDefinition<Coffee>)JsonSerializer.Serialize(new { Id = coffeeId });

        return coffees.Find(jsonFind).SingleOrDefault();
    }

    public IReadOnlyCollection<Coffee> GetAll()
     => coffees.Find((FilterDefinition<Coffee>)"{}").ToList();
}
