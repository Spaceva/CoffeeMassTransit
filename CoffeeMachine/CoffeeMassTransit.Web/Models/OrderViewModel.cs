using System.ComponentModel.DataAnnotations;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Web
{
    public class OrderViewModel
    {
        [Required]
        public CoffeeType CoffeeType { get; set; }

        [Required]
        public string Name { get; set; }

        public bool WithWhippedCream { get; set; }

        public bool WithCaramel { get; set; }

        public bool WithChocolate { get; set; }

        public bool WithWhiskey { get; set; }
    }
}
