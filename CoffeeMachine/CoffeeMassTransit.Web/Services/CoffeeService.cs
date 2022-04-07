using System;
using System.Collections.Generic;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;

namespace CoffeeMassTransit.Web;

public class CoffeeService
{
    private readonly ICoffeeRepository coffeeRepository;

    public CoffeeService(ICoffeeRepository coffeeRepository)
    {
        this.coffeeRepository = coffeeRepository;
    }

    public Coffee Get(Guid coffeeId) => coffeeRepository.Get(coffeeId);

    public IReadOnlyCollection<Coffee> GetAll() => coffeeRepository.GetAll() ?? Array.Empty<Coffee>();
}
