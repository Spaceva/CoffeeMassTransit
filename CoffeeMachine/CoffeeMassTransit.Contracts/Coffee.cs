using System;
using System.Collections.Generic;

namespace CoffeeMassTransit.Contracts;

public class Coffee
{
    public Guid Id { get; init; }

    public CoffeeType Type { get; init; }

    public List<Topping> Toppings { get; init; } = new List<Topping>();

    public bool Done { get; init; }
}
