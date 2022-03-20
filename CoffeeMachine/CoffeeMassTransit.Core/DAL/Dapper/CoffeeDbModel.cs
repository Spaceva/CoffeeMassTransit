using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Core.DAL;

public class CoffeeDbModel
{
    public Guid Id { get; init; }
    
    public CoffeeType Type { get; init; }

    public string Toppings { get; init; } = default!;

    public bool Done { get; init; }
}
