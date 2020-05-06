using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Core.DAL
{
    public class CoffeeDbModel
    {
        public Guid Id { get; set; }
        public CoffeeType Type { get; set; }
        public string Toppings { get; set; }

        public bool Done { get; set; }
    }
}
