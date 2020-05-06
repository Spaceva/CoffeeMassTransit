using System;
using System.Collections.Generic;

namespace WebinarMassTransit.Contracts
{
    public class Coffee
    {
        public Guid Id { get; set; }
        public CoffeeType Type { get; set; }
        public List<Topping> Toppings { get; set; }

        public bool Done { get; set; }
    }
}
