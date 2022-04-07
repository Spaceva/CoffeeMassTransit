using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Core;

public static class CoffeePriceCalculator
{
    public static float Compute(CoffeeType coffeeType, params Topping[] toppings)
    {
        float price = GetBasePrice(coffeeType);

        if (toppings is null)
        {
            return price;
        }

        foreach (var topping in toppings)
        {
            price += GetToppingPrice(topping);
        }

        return price;
    }

    private static float GetToppingPrice(Topping topping)
    {
        return topping switch
        {
            Topping.Caramel => 0.25f,
            Topping.Chocolate => 0.25f,
            Topping.Whiskey => 0.7f,
            Topping.WhippedCream => 0.1f,
            _ => 0,
        };
    }

    private static float GetBasePrice(CoffeeType coffeeType)
    {
        return coffeeType switch
        {
            CoffeeType.Americano => 0.75f,
            CoffeeType.Black => 0.50f,
            CoffeeType.Cappucino => 0.72f,
            CoffeeType.Espresso => 0.55f,
            CoffeeType.Irish => 0.75f,
            CoffeeType.Latte => 0.60f,
            CoffeeType.Lungo => 0.59f,
            CoffeeType.Macchiato => 0.67f,
            CoffeeType.Mocha => 0.80f,
            CoffeeType.Ristretto => 0.54f,
            _ => 0,
        };
    }
}
