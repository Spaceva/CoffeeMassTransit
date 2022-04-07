using System;
using System.Collections.Generic;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;

namespace CoffeeMassTransit.Core;

public class CoffeeInMemoryRepository : ICoffeeRepository
{
    private readonly Dictionary<Guid, Coffee> Data = new();

    public void AddToppings(Guid coffeeId, IReadOnlyCollection<Topping> toppings)
    {
        var coffee = Get(coffeeId);
        if (toppings == null)
        {
            throw new ArgumentNullException(nameof(toppings));
        }

        coffee.Toppings.AddRange(toppings);
    }

    public void Create(Guid coffeeId, Guid orderId, CoffeeType coffeeType, bool noTopping)
    {
        Data.Add(coffeeId, new Coffee { Id = coffeeId, OrderId = orderId, Type = coffeeType, Toppings = new List<Topping>(), Done = noTopping });
    }

    public Coffee Get(Guid coffeeId)
    {
        if (!Data.TryGetValue(coffeeId, out Coffee? coffee))
        {
            throw new ArgumentOutOfRangeException(nameof(coffeeId));
        }

        return coffee;
    }

    public IReadOnlyCollection<Coffee> GetAll()
    {
        return Data.Values;
    }
}
