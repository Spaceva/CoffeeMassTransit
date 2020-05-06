using System;
using System.Collections.Generic;
using WebinarMassTransit.Contracts;
using WebinarMassTransit.Core;

namespace WebinarMassTransit.Web
{
    public class CoffeeService
    {
        private readonly ICoffeeRepository coffeeRepository;

        public CoffeeService(ICoffeeRepository coffeeRepository)
        {
            this.coffeeRepository = coffeeRepository;
        }

        public Coffee Get(Guid coffeeId) => this.coffeeRepository.Get(coffeeId);
        public IReadOnlyCollection<Coffee> GetAll() => this.coffeeRepository.GetAll() ?? new Coffee[0];
    }
}
